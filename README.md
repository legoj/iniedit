# iniedit
Commandline utility to update/modify ini file. This simple program can be included into your software deployment package and invoke via install scripts to automate modifying the ini file with values which can only be determined during deployment runtime. Or can also be used in deploying configuration updates to existing ini files on the target systems using simple update scripts. 


## Usage:
```
>iniedit iniFilePath [actionVerb] [actionVerbParameters...] [behaviorFlag]

Where:
 iniFilePath - fullPath of the ini file
 actionVerb - could be one of the following:
    Add/Overwrite a prop Name/Value into a specific section.
     /a [sectionName]:[propName]=[propValue]
    Add a section.
     /a [sectionName]
    Delete a prop under a specific section.
     /d [sectionName]:[propName]
    Delete a whole section.
     /d [sectionName]
 behaviorFlag - either of the following:
    /y - If file does not exist, /a will create new ini file; /d is ignored.
         if not specified, return an error code -1 if file does not exist.
```
