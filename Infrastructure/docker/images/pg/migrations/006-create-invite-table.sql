CREATE TABLE invite (
projectID uuid REFERENCES project(projectID),
freelancerID uuid REFERENCES  freelancer(userID),
PRIMARY KEY (projectID, freelancerID)
);
