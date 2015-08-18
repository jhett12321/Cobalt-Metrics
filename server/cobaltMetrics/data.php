<?php
header('Content-Type: application/json');

$method = $_SERVER['REQUEST_METHOD'];

//DEBUG
//$method = 'POST';

if($method === 'POST')
{
	//Verify received data.
	$data = json_decode(file_get_contents('php://input'));
	
	//DEBUG
	//$data = json_decode('{"session_info":{"user_key":"example","session_id":"00000000000000000000000000000001"},"data":{"key":"someData","timestamp":"1439102738041","type":"array","values":["someValue1","someValue2","someValue3","someValue4"]}}');
	
	// $data is null because the json cannot be decoded
	if($data === null)
	{
		ErrorResponse(400, 'You have not provided any data, or it is corrupt. Please try again with valid data.');
	}
	
	// Validate all required JSON elements exist.
	if (!isset($data->session_info->user_key) ||
		!isset($data->session_info->session_id) ||
		!isset($data->data->key) ||
		!isset($data->data->timestamp) ||
		!isset($data->data->type) ||
		(!isset($data->data->values) && !isset($data->data->value)))
	{
		ErrorResponse(400, 'A required post field is missing. Please check your posted data.');
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
			ErrorResponse(403, 'The user key provided does not exist, or is currently disabled.');
		}
		
		$query = $db->prepare('SELECT * FROM `sessions` WHERE `id` = :sessionID');
		$query->bindParam(':sessionID', $data->session_info->session_id, PDO::PARAM_STR);
		$query->execute();
		
		$result = $query->fetch(PDO::FETCH_ASSOC);
		
		//We don't have an existing session.
		if(!$result)
		{
			ErrorResponse(412, 'The provided session ID does not exist. Please start a session with the given ID and try adding data again.');
		}
		
		//Insert Data into DB
		switch($data->data->type)
		{
			case 'single':
			{
				$typeID = 1;
				$i = 0;
				
				$query = $db->prepare('INSERT IGNORE INTO `data` (`id`, `user_key`, `session_id`, `type_id`, `array_index`, `value`, `timestamp`) VALUES (:dataID, :userKey, :sessionID, :typeID, :arrayIndex, :value, :timestamp)');
				$query->bindParam(':dataID', $data->data->key, PDO::PARAM_STR);
				$query->bindParam(':userKey', $data->session_info->user_key, PDO::PARAM_STR);
				$query->bindParam(':sessionID', $data->session_info->session_id, PDO::PARAM_STR);
				$query->bindParam(':typeID', $typeID, PDO::PARAM_INT);
				$query->bindParam(':arrayIndex', $i, PDO::PARAM_INT);
				$query->bindParam(':value', $data->data->value, PDO::PARAM_STR);
				$query->bindParam(':timestamp', $data->data->timestamp, PDO::PARAM_STR);
				
				$query->execute();
				break;
			}
			case 'array':
			{
				$typeID = 2;
				
				for($i = 0; $i < count($data->data->values); ++$i)
				{
					$query = $db->prepare('INSERT IGNORE INTO `data` (`id`, `user_key`, `session_id`, `type_id`, `array_index`, `value`, `timestamp`) VALUES (:dataID, :userKey, :sessionID, :typeID, :arrayIndex, :value, :timestamp)');
					$query->bindParam(':dataID', $data->data->key, PDO::PARAM_STR);
					$query->bindParam(':userKey', $data->session_info->user_key, PDO::PARAM_STR);
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
		
		//Query Limits
		$end = 1000;
		
		if(!empty($_GET['limit']) && $_GET['limit'] < 3000)
		{
			$end = $_GET['limit'];
		}
		
		if(!empty($_GET['start']))
		{
			$end += $_GET['start'];
		}
		
		//Optional Params
		$sessionID = !empty($_GET['session_id']) ? $_GET['session_id'] : null;
		$dataID = !empty($_GET['data_id']) ? $_GET['data_id'] : null;
		$after = !empty($_GET['after']) ? $_GET['after'] : null;
		$before = !empty($_GET['before']) ? $_GET['before'] : null;
		$start = !empty($_GET['start']) ? $_GET['start'] : null;
		
		$sql = sprintf('SELECT * FROM `data` WHERE `user_key` = :userKey%s%s%s%s%s ORDER BY `timestamp` DESC;',
					   !empty($sessionID) ? ' AND `session_id` = :sessionID' : null,
					   !empty($dataID) ? ' AND `id` = :dataID' : null,
					   !empty($after) ? ' AND `timestamp` > :after' : null,
					   !empty($before) ? ' AND `timestamp` < :before' : null,
					   !empty($start) ? ' LIMIT :start, :end' : ' LIMIT :end');
		
		$query = $db->prepare($sql);
		
		$query->bindParam(':userKey', $userKey, PDO::PARAM_STR);

		if (!empty($sessionID))
		{
			$query->bindParam(':sessionID', $sessionID, PDO::PARAM_STR);
		}
		
		if (!empty($dataID))
		{
			$query->bindParam(':dataID', $dataID, PDO::PARAM_STR);
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
			'returned' => count($results)
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
