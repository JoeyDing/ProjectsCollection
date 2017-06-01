//import * as $ from 'jquery'

var sprintNumber = '';
var taskidsArray = new Array();
//save the input sprint number into global variable
$(document).ready(function(){
   $("#sprintID").on("change paste keyup", function() {
       sprintNumber = $(this).val();
       alert("Input value is :"+sprintNumber);   
   });
});

//collect all the items ids from vso
 $(document).ready(function(){
  var customQueryStr = "select [System.Id] from WorkItems where ([System.TeamProject] = 'LOCALIZATION') and ([System.WorkItemType] = 'Task') and ([System.AreaPath] UNDER 'LOCALIZATION\\Intl Tools') and ([System.IterationPath] = 'LOCALIZATION\\Intl Tools Backlog\\Intl Tools S62') and([System.State] = 'Resolved') AND ( [System.AssignedTo] = 'Joey Ding <v-joding@microsoft.com>')";
  //var json_text = JSON.stringify(customQuery);
  var customQueryData = { "query" : customQueryStr };
  $.ajax({ 
     type:'Post',
     contentType:"application/json",
     headers: {          
         Accept: "application/json",            
     },     
     url:'https://skype.visualstudio.com/DefaultCollection/LOCALIZATION/_apis/wit/wiql?api-version=1.0',
     data: JSON.stringify({"query":customQueryStr}),
     dataType: 'json',
     beforeSend: function (xhr) {
        xhr.setRequestHeader ("Authorization", "Basic OmxzcGIzbDZxZWN6MjNjNmt0b3dnNmh2c3FuMmlka3RpanNwemg3a3FmcG1wbnpmMmR5ZXE=");
     },
     success: function (data) {
        var parsed = jQuery.parseJSON(JSON.stringify(data)); 
        parsed["workItems"].forEach(element => {
          taskidsArray.push(element["id"]);
        });
     } 
  })
 });
 
//get vso work items of current sprint
$(document).ready(function(){
$("#getItemsButton").click(function(){

    $.ajax({
    type: 'GET',
    accepts:'application/json',
    url: 'https://skype.visualstudio.com/DefaultCollection/_apis/wit/workitems?ids=677085&fields=System.Id,Microsoft.VSTS.Scheduling.OriginalEstimate&api-version=1.0',
    dataType: 'json',
    beforeSend: function (xhr) {
        xhr.setRequestHeader ("Authorization", "Basic OmxzcGIzbDZxZWN6MjNjNmt0b3dnNmh2c3FuMmlka3RpanNwemg3a3FmcG1wbnpmMmR5ZXE=");
    },
    success: function (data) {
        var parsed = jQuery.parseJSON(JSON.stringify(data)); 
        alert("data : "+parsed);
    }
})
});
});

/*
$(document).ready((jquery)=>{

var query = "https://skype.visualstudio.com/DefaultCollection/_apis/wit/workitems?ids=677085&$expand=all&api-version=1.0";
//$.get("https://skype.visualstudio.com/DefaultCollection/_apis/wit/workitems?ids=677085&$expand=all&api-version=1.0");
var jsonText ='{"count":1,"value":[{"id":677085,"rev":3,"fields":{"System.Id":677085,"System.State":"In Progress","System.AssignedTo":"Joey Ding","System.Title":"GarageDayProject-VSO-Task-Logger: Implement ""GetWorkItemsFromVSO"" event by using Jquery function","Microsoft.VSTS.Scheduling.OriginalEstimate":7.0,"Microsoft.VSTS.Scheduling.RemainingWork":7.0,"Microsoft.VSTS.Scheduling.CompletedWork":0.0},"url":"https://skype.visualstudio.com/DefaultCollection/_apis/wit/workItems/677085"}]}';

var jsonObj = jquery.parseJSON(jsonText); 
 

 //$('#SprintName')[0].innerHTML = 'blabla'; 

var assignedTo = jsonObj.value[0].fields["System.AssignedTo"]; 
var id = jsonObj.value[0].fields["System.Id"]; 
var status = jsonObj.value[0].fields["System.State"];
var remainingWork = jsonObj.value[0].fields["Microsoft.VSTS.Scheduling.RemainingWork"];
var originalEstimate = jsonObj.value[0].fields["Microsoft.VSTS.Scheduling.OriginalEstimate"];
var completedWork = jsonObj.value[0].fields["Microsoft.VSTS.Scheduling.CompletedWork"];


$('#TaskID').text(id); 
$('#TaskStatus').text(status);
$('#EstimatedHours').text(originalEstimate);
$('#RemainingHours').text(remainingWork);
$('#CompletedHours').text('+'+ completedWork);
//$('#CompletedHoursValueID').val(completedWork);

});
*/