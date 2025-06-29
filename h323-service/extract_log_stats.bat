REM Extract statistics log messages from H.323 service logs and scrub
REM them so they can be formatted into a nice CSV file for viewing in 
REM Microsoft Excel.
REM
REM Usage: Run this batch file in the log directory with the logs

grep STAT *.log | gawk "{print $4,$6,$7;}" | sed -e s/" "/","/g > stat.csv