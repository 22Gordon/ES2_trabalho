-- Insert initial data into the user table                                                                                                       
INSERT INTO users (userID, displayName, username, password)
VALUES ('d6054b95-33ca-4a79-86c3-5d80a1f9d678', 'John Doe', 'johndoe', 'password123');

INSERT INTO users (userID, displayName, username, password)
VALUES ('8f6198b3-0760-4ab0-b1f4-6e79890d317a', 'Jane Smith', 'janesmith', 'pass456');

INSERT INTO users (displayName, username, password)
VALUES ('Alice Johnson', 'alicej', 'abc789');

-- Insert initial data into the client table                                                                                                         
INSERT INTO client (userID)
VALUES ('d6054b95-33ca-4a79-86c3-5d80a1f9d678');

-- Insert initial data into the freelancer table                                                                                                         
INSERT INTO freelancer (userID, dailyAvgHours)
VALUES ('8f6198b3-0760-4ab0-b1f4-6e79890d317a', 5.5);

-- Insert initial data into the project table  
INSERT INTO project (projectID, name, projectLeaderID, clientID, priceHour)
VALUES ('507d42fa-aa17-4478-ac2f-8ea8e31cd738', 'Project A', '8f6198b3-0760-4ab0-b1f4-6e79890d317a', 'd6054b95-33ca-4a79-86c3-5d80a1f9d678', 50.0);

-- Insert initial data into the invite table
INSERT INTO invite (projectID, freelancerID, isAccepted)
VALUES ('507d42fa-aa17-4478-ac2f-8ea8e31cd738', '8f6198b3-0760-4ab0-b1f4-6e79890d317a', true);

INSERT INTO user_task (freelancerID, clientID, startDate, endDate, priceHour, title, description)
VALUES ('8f6198b3-0760-4ab0-b1f4-6e79890d317a', 'd6054b95-33ca-4a79-86c3-5d80a1f9d678', '2023-06-01 09:00:00',null,  25.0, 'Título da Tarefa', 'Descrição da Tarefa');