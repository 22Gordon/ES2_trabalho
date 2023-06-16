CREATE TABLE user_task (
taskID      uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
freelancerID    uuid,
clientID    uuid,
startDate   TIMESTAMP WITH TIME ZONE,
endDate     TIMESTAMP WITH TIME ZONE,
title       VARCHAR(255),
description TEXT,
priceHour   FLOAT,
FOREIGN KEY (freelancerID) REFERENCES freelancer (userID),
FOREIGN KEY (clientID) REFERENCES client (userID)
);
