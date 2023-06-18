CREATE TABLE taskproject (
taskID uuid REFERENCES user_task(taskID),
projectID uuid REFERENCES project(projectid),
PRIMARY KEY (taskID, projectID)
);
