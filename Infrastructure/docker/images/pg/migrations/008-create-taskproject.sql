CREATE TABLE taskproject (
taskID uuid REFERENCES user_task(taskID),
projectID uuid REFERENCES project(projectid),
aux_column integer,
PRIMARY KEY (taskID, projectID)
);
