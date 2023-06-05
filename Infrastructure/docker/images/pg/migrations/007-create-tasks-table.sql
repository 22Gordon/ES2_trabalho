CREATE TABLE task (
taskID         uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
projectID      uuid,
freelancerID   uuid,
startDate      TIMESTAMP,
endDate        TIMESTAMP,
priceHour      FLOAT,
FOREIGN KEY (projectID) REFERENCES project (projectID),
FOREIGN KEY (freelancerID) REFERENCES freelancer (userID)
);
