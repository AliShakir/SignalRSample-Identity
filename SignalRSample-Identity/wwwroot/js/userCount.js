//create a connection
var connectionUserCount = new signalR.HubConnectionBuilder().withUrl("/hubs/userCount").build();

//connect to methods that hub invokes aka receive notification from hub
connectionUserCount.on("updateTotalViews", (value) => {
    var newCountSpan = document.getElementById("totalViewsCounter");
    newCountSpan.innerHTML = value.toString();
});

//
connectionUserCount.on("updateTotalUsers", (value) => {
    var newCountSpan = document.getElementById("totalUserCounter");
    newCountSpan.innerHTML = value.toString();
});

//invode hub methods aka send notification to hub
function newWindowLoadedOnClient() {
    connectionUserCount.send("NewWindowLoaded")
}
//start connection
function fulfilled() {
    console.log("Connection successfull");
    newWindowLoadedOnClient();
}
//
function rejected() {
    
}
connectionUserCount.start().then(fulfilled, rejected);