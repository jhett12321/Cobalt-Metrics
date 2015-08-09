<?php
$method = $_SERVER['REQUEST_METHOD'];

//DEBUG
//$method = "POST";

if($method === "POST")
{
	//Verify received data.
	$data = json_decode(file_get_contents('php://input'));
	
	//DEBUG
	//$data = json_decode('{"session_info":{"user_key":"example","session_id":"00000000000000000000000000000000"},"data":{"key":"someData","timestamp":"1439102738041","type":"array","values":["someValue1","someValue2","someValue3","someValue4"]}}');
	
	// $data is null because the json cannot be decoded
	if($data === null)
	{
		http_response_code(400);
		exit;
	}
	
	// Validate all required JSON elements exist.
	if (!isset($data->session_info->user_key) ||
		!isset($data->session_info->session_id) ||
		!isset($data->data->key) ||
		!isset($data->data->timestamp) ||
		!isset($data->data->type) ||
		(!isset($data->data->values) && !isset($data->data->value)))
	{
		http_response_code(400);
		exit;
	}
	
	//Create our database connection
	try
	{
		$db = new PDO('mysql:dbname=cobalt_metrics;host=127.0.0.1', 'cobaltMetrics', 'studio2abc@22@#d.');
		
		//Validate this user is approved, and has a valid session
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
		
		$query = $db->prepare('SELECT * FROM `sessions` WHERE `id` = :sessionID');
		$query->bindParam(':sessionID', $data->session_info->session_id, PDO::PARAM_STR);
		$query->execute();
		
		$result = $query->fetch(PDO::FETCH_ASSOC);
		
		//We don't have an existing session.
		if(!$result)
		{
			http_response_code(412);
			exit;
		}
		
		//Insert Data into DB
		switch($data->data->type)
		{
			case "single":
			{
				$typeID = 1;
				$i = 0;
				
				$query = $db->prepare('INSERT IGNORE INTO `data` (`id`, `session_id`, `type_id`, `array_index`, `value`, `timestamp`) VALUES (:dataID, :sessionID, :typeID, :arrayIndex, :value, :timestamp)');
				$query->bindParam(':dataID', $data->data->key, PDO::PARAM_STR);
				$query->bindParam(':sessionID', $data->session_info->session_id, PDO::PARAM_STR);
				$query->bindParam(':typeID', $typeID, PDO::PARAM_INT);
				$query->bindParam(':arrayIndex', $i, PDO::PARAM_INT);
				$query->bindParam(':value', $data->data->value, PDO::PARAM_STR);
				$query->bindParam(':timestamp', $data->data->timestamp, PDO::PARAM_STR);
				
				$query->execute();
				break;
			}
			case "array":
			{
				$typeID = 2;
				
				for($i = 0; $i < count($data->data->values); ++$i)
				{
					$query = $db->prepare('INSERT IGNORE INTO `data` (`id`, `session_id`, `type_id`, `array_index`, `value`, `timestamp`) VALUES (:dataID, :sessionID, :typeID, :arrayIndex, :value, :timestamp)');
					$query->bindParam(':dataID', $data->data->key, PDO::PARAM_STR);
					$query->bindParam(':sessionID', $data->session_info->session_id, PDO::PARAM_STR);
					$query->bindParam(':typeID', $typeID, PDO::PARAM_INT);
					$query->bindParam(':arrayIndex', $i, PDO::PARAM_INT);
					$query->bindParam(':value', $data->data->values[$i], PDO::PARAM_STR);
					$query->bindParam(':timestamp', $data->data->timestamp, PDO::PARAM_STR);
					
					$query->execute();
				}
				
				break;
			}
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
