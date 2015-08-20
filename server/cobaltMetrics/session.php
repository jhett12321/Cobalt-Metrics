<?php
header('Content-Type: application/json');

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
		ErrorResponse(400, 'You have not provided any data, or it is corrupt. Please try again with valid data.');
	}
	
	// Validate all required JSON elements exist.
	if (!isset($data->session_info->user_key) ||
		!isset($data->session_info->session_id) ||
		(!isset($data->session_info->start_time) && !isset($data->session_info->end_time)))
	{
		ErrorResponse(400, 'A required post field is missing. Please check your posted data.');
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
			ErrorResponse(403, 'The user key provided does not exist, or is currently disabled.');
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
		ErrorResponse(500, 'Internal DB Exception Occurred.');
	}        
	
	exit;
}

else if($method === 'GET')
{
	if(!isset($_GET['user_key']))
	{
		ErrorResponse(401, 'You have not provided any user key. Please include your user key with your query, and try again.');
	}
	
	$userKey = $_GET['user_key'];
	
	//DEBUG
	//$userKey = 'example';
	
	//Create our database connection
	try
	{
		$db = new PDO('mysql:dbname=cobalt_metrics;host=127.0.0.1', 'cobaltMetrics', 'studio2abc@22@#d.');
		
		//Validate this user is approved, and has a valid session
		$query = $db->prepare('SELECT * FROM `users` WHERE `key` = :userKey AND `enabled` = 1');
		$query->bindParam(':userKey', $userKey, PDO::PARAM_STR);
		$query->execute();
		
		$result = $query->fetch(PDO::FETCH_ASSOC);
		
		//This user doesn't exist, or is currently disabled.
		if(!$result)
		{
			ErrorResponse(403, 'The user key provided does not exist, or is currently disabled.');
		}
		
		//Query database based on the query parameters.
		$queryStart = microtime(true);
		
		//Query Limits
		$end = 1000; //Default Value
		
		if(!empty($_GET['limit']) && $_GET['limit'] < 10000)
		{
			$end = intval($_GET['limit']);
		}
		
		else if(!empty($_GET['limit']))
		{
			$end = 10000;
		}
		
		if(!empty($_GET['start']))
		{
			$end += $_GET['start'];
		}
		
		//Optional Params
		$sessionID = !empty($_GET['session_id']) ? $_GET['session_id'] : null;
		$after = !empty($_GET['after']) ? $_GET['after'] : null;
		$before = !empty($_GET['before']) ? $_GET['before'] : null;
		$start = !empty($_GET['start']) ? $_GET['start'] : null;
		
		$sql = sprintf('SELECT * FROM `sessions` WHERE `user_key` = :userKey%s%s%s ORDER BY `start_time` DESC%s;',
					   !empty($sessionID) ? ' AND `session_id` = :sessionID' : null,
					   !empty($after) ? ' AND `start_time` > :after' : null,
					   !empty($before) ? ' AND `start_time` < :before' : null,
					   !empty($start) ? ' LIMIT :start, :end' : ' LIMIT :end');
		
		$query = $db->prepare($sql);
		
		$query->bindParam(':userKey', $userKey, PDO::PARAM_STR);

		if (!empty($sessionID))
		{
			$query->bindParam(':sessionID', $sessionID, PDO::PARAM_STR);
		}
		
		if (!empty($after))
		{
			$query->bindParam(':after', $after, PDO::PARAM_STR);
		}
		
		if (!empty($before))
		{
			$query->bindParam(':before', $before, PDO::PARAM_STR);
		}
		
		if (!empty($start))
		{
			$query->bindParam(':start', $start, PDO::PARAM_INT);
		}
		
		$query->bindParam(':end', $end, PDO::PARAM_INT);
		
		$query->execute();
		$results = $query->fetchAll(PDO::FETCH_ASSOC);
		
		//Create the parent JSON element
		$returnData = array(
			'data_list' => $results,
			'returned' => count($results),
			'query_ms' => round((microtime(true) - $queryStart) * 1000)
		);
		
		print(json_encode($returnData));
		exit;
	}
	
	catch (PDOException $e)
	{
		ErrorResponse(500, 'Internal DB Exception Occurred.');
	}        
}

else
{
	ErrorResponse(501, 'Unknown Request Method. ' . $_SERVER['REQUEST_METHOD'] . ' Not Implemented.');
}

function ErrorResponse($errCode, $errMsg)
{
	http_response_code($errCode);
	
	$returnData = array(
		'response_code' => $errCode,
		'message' => $errMsg
	);
	
	print(json_encode($returnData));
	exit;
}
?>
