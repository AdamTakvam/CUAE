## This script generates the datafile needed for the BAT tool in CCM
## to create CTI Ports using BAT

## NOTE: If you need more than 100 CTI ports, you must create them in batches
## of 100 because BAT or CCM won't work if you try to create more at one time.

## direct the output of this file to a file: mydatafile.csv

## myctiport = CTI Port name
## myctiport1, myctiport2...

## myctidescription is the CTI port description

## 1300 is the base phone number

for i in range(0, 100):
    print "1,myctiport"+repr(i+1)+",myctidescription,"+repr(1300+i)
