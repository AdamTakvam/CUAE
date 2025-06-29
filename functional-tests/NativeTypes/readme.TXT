Note: 
- This test is executed using a URL to the appliance and passing in a DN to call
i.e.  http://clarke:8000/NativeTest?To=12808

- MAX-242: Since the assign function will always return 'true'
The developer who created the native type must create a parse 
statement in the clase definition to handle non-strings.

- String parse method of the QAtype class will return 'False'
if it finds "INVALID" in the string. The fix to SMA-1125
will then cause the script to exit and report a failure.

- The class also contains a parse method for an object and
verifies if the object contains the string "INVALID". If found
a false is returned.

So at each point where you assign a string to the QAtype, if that string contains the
characters "INVALID" the script should exit, otherwise it should continue.

The is a config value called AssignTest where the user can set the string value.
set it to something other then 'INVALID' and the script should pass.

This test application has 4 seperate tests:
- Set the default value for the QAtype  (config value - DefaultTest)
- Place the result data CallId into the QAtype
- Use the Assign Action to assign a QAtype with a value (config value - AssignTest)
- Place the rsult data from the NativeAction FileSize into the QAtype