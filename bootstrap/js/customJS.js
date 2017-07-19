var spanWidth = $('#animatedText span').width();
$('#animatedText').animate( { width: spanWidth }, 3000 );


var myCat = {
   "name":"one",
   "species":"cat",
   "favFood":"tuna"
}

var maFavColor = ["blue","green","purpkle"];

var thePets =[
    {
    "name":"one",
    "species":"cat",
    "favFood":"tuna"
    },
    {
    "name":"two",
    "species":"dog",
    "favFood":"carrots"
    }
]

var pageCounter =1;
var animalContainer = document.getElementById("animal-info");
var btn = document.getElementById("btn");

btn.addEventListener("click",function(){
    //ajax means (asynchronous javascript and xml)
    //async: in the background, not require page refresh
    //below a browwser buildint tool is used to download file through url
    var ourRequest = new XMLHttpRequest();
    ourRequest.open('GET','https://learnwebcode.github.io/json-example/animals-' + pageCounter + '.json');
    ourRequest.onload = function(){
        //ERROR handling -1 
        //check if able to see the data
        if(ourRequest.status >=200 && ourRequest.status <400 ){
            //log out the json dat we downloaded and show it through concole
            //console.log(ourRequest.responseText);
            var ourData = ourRequest.responseText;
            //tell the browser this is json data, not just plain text.as above
            var ourJsonData = JSON.parse(ourData);
            //for tesing purpose
            //console.log(ourJsonData[0])
            renderHTML(ourJsonData);    
        }else{
            console.log("We connected to the server, but it returned on error.")
        }

    };
   
   //ERROR handling -2 
    ourRequest.onerror = function(){
      console.log("Conenction error");
    }

    //send the request
    ourRequest.send();
    pageCounter++;
    if(pageCounter > 3){
        btn.classList.toggle("hide");
    }
});

//purpose is to add html into div of animal-info
function renderHTML(data){
    //var htmlString = "this is a test";
    var htmlString = "";
    for(i=0;i<data.length;i++){
      //htmlString +="<p>" + data[i].name + " is a "+ data[i].species + ".</p>";
      htmlString +="<p>" + data[i].name + " is a "+ data[i].species + " that likes to eat ";
      for(j =0;j < data[i].foods.likes.length;j++){
          if(j==0){
              htmlString += data[i].foods.likes[j];
          }else{
              htmlString += " and " + data[i].foods.likes[j];
          }
      }

      htmlString +='and dislikes ';
      for(k =0;k < data[i].foods.dislikes.length;k++){
          if(k==0){
              htmlString += data[i].foods.dislikes[k];
          }else{
              htmlString += " and " + data[i].foods.dislikes[k];
          }
      }
      htmlString +='.</p>';
    }
  animalContainer.insertAdjacentHTML('beforeend',htmlString);
}

