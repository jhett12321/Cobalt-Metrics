<?php
$method = $_SERVER['REQUEST_METHOD'];

//DEBUG
//$method = "POST";

if($method === "POST")
{
	//Verify received data.
	$data = json_decode(file_get_contents('php://input'));
	
	//DEBUG
	//$data = json_decode('{"session_info":{"user_key":"example","session_id":"00000000000000000000000000000001","start_time":"1439102738041"}}');
	
	// $data is null because the json cannot be decoded
	if($data === null)
	{
		http_response_code(400);
		exit;
	}
	
	// Validate all required JSON elements exist.
	if (!isset($data->session_info->user_key) ||
		!isset($data->session_info->session_id) ||
		(!isset($data->session_info->start_time) && !isset($data->session_info->end_time)))
	{
		http_response_code(400);
		exit;
	}
	
	//Create our database connection
	try
	{
		$db = new PDO('mysql:dbname=cobalt_metrics;host=127.0.0.1', 'cobaltMetrics', 'studio2abc@22@#d.');
		
		//Validate this user, and check they have been granted access.
		$query = $db->prepare('SELECT * FROM `users` WHERE `key` = :userKey AND `enabled` = 1');
		$query->bindParam(':userKey', $data->session_info->user_key, PDO::PARAM_STR);
		$query->execute();
		
		$result = $query->fetch(PDO::FETCH_ASSOC);
		
		//This user doesn't exist, or is currently disabled.
		if(!$result)
		{
			http_response_code(403);
			exit;
		}
		
		$postType = 0;
		
		if(isset($data->session_info->end_time))
		{
			$postType = 1;
		}
		
		//We need to create a new session
		if($postType == 0)
		{
			$query = $db->prepare('INSERT IGNORE INTO `sessions` (`id`, `user_key`, `start_time`) VALUES (:sessionID, :userKey, :startTime)');
			$query->bindParam(':sessionID', $data->session_info->session_id, PDO::PARAM_STR);
			$query->bindParam(':userKey', $data->session_info->user_key, PDO::PARAM_STR);
			$query->bindParam(':startTime', $data->session_info->start_time, PDO::PARAM_STR);
			
			$query->execute();
		}
		
		//This session has ended.
		else
		{
			$query = $db->prepare('UPDATE `sessions` SET `end_time` = :endTime WHERE `id` = :sessionID');
			$query->bindParam(':sessionID', $data->session_info->session_id, PDO::PARAM_STR);
			$query->bindParam(':endTime', $data->session_info->end_time, PDO::PARAM_STR);
			
			$query->execute();
		}
	}
	catch (PDOException $e)
	{
		http_response_code(500);
		exit();
	}        
	
	$db = null; //Close DB connection.
	http_response_code(200);
	exit;
}

else
{
	http_response_code(501);
	exit;
}
?>
