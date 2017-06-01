## Depolyment For Test project(CS type) 

>Version：618a0335a7412ec328474444c24fca45c493ad36

#### Prepare deployment files：

1. Code have been tested on local machine
2. Code are pulled to git
	
	
#### deployment target enviornment: \\skypeintl\ASPNETApps\Portfolio


#### In progress in deployment, Should do operation according following table. Mark after each step once finish them.

### 1. Deployment steps

| Step Order | Step Content                | Check Column |
| ---------- |:--------------------------- | ------------ | 
| 1          | Confirm the deployment time |              |
| 2          | Log in VM, pull the latest code under \internal_tools_intltools\SkypeIntlPortfolio\SkypeIntlPortfolio.Ajax\ |              |
| 3          | Open project in VisualStudio |    |
| 4     | Clean and Rebuild on Release mode |   |
| 5     | Check: JobHistory page can display on local in debuging mode|  |
| 6     | Stop debuging |  |
| 7     | Right click on project "SkypeIntlPortfolio.Ajax", click on "Publish" |     |
| 8     | Make sure the target location is "\\skypeintl\ASPNETApps\Portfolio"  |   |
| 9     | Male sure the configuration is "Release" |    |
| 10     | Click "Publish" button|    |
| 11     | After publish finished, wait for 2 minutes |    |
| 12     | Go to "http://skypeintl/portfolio/Pages/Monitor/JobCurrentStatusNew.aspx" |    |
| 13    | Check jobHistory page can work correctly |    |


### 2. Deployment Succeed Notification

| Step Order | Step Content                | 
| ---------- |:--------------------------- |  
| 1          | No need for this |    |

### 3. Frequently Asked Questions 

| ID| Questions               | Answer |
| ---------- |:--------------------------- | ------------ | 
| 1          | What need to do if error happens in publish progress |  open both source folder and target folder, delete all files and rebuild again |


