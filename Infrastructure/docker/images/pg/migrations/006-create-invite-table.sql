CREATE TABLE invite (
projectID uuid REFERENCES project(projectID),
freelancerID uuid REFERENCES freelancer(userID),
isAccepted boolean NOT NULL DEFAULT false,
PRIMARY KEY (projectID, freelancerID)
);
