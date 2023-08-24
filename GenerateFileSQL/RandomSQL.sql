--1mil

--with index
--CPU: 1767
--Reads: 13920
--Duration: 518

--without index
--CPU: 1188
--Reads: 14090
--Duration: 476

--10mil

--with index
--CPU: 13172
--Reads: 127519
--Duration: 1429

--without index
--CPU: 8094
--Reads: 128384
--Duration: 896

--select any number
SELECT
    *
FROM
    Random
WHERE
    RandomNumbers = 1234;



--Group the count of the random numbers
SELECT
    COUNT(*) AS 'Appears'
FROM
    Random
GROUP BY
    RandomNumbers;



--what is the most freequent number?
SELECT
    TOP 1 RandomNumbers,
    COUNT(*) AS Frequency
FROM
    Random
GROUP BY
    RandomNumbers
ORDER BY
    Frequency DESC;



--what is the least frequent number
SELECT
    TOP 1 RandomNumbers,
    COUNT(*) AS Frequency
FROM
    Random
GROUP BY
    RandomNumbers
ORDER BY
    Frequency ASC;



--what is the most freequent number?
WITH CountedValues AS (
    SELECT
        RandomNumbers,
        COUNT(*) AS Frequency
    FROM
        Random
    GROUP BY
        RandomNumbers
)
SELECT
    RandomNumbers
FROM
    CountedValues
WHERE
    Frequency = (
        SELECT
            MAX(Frequency)
        FROM
            CountedValues
    );



--what is the least freequent number?
WITH CountedValues AS (
    SELECT
        RandomNumbers,
        COUNT(*) AS Frequency
    FROM
        Random
    GROUP BY
        RandomNumbers
)
SELECT
    RandomNumbers
FROM
    CountedValues
WHERE
    Frequency = (
        SELECT
            MIN(Frequency)
        FROM
            CountedValues
    );