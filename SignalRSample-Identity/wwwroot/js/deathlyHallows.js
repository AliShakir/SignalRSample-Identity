

var cloakSpan = document.getElementById("cloakCounter");
var stoneSpan = document.getElementById("stoneCounter");
var wandSpan = document.getElementById("wandCounter");

//create a connection
var connectionDeathlyHallows = new signalR.HubConnectionBuilder()
                            .withUrl("/hubs/deathlyHallows").build();

//connect to methods that hub invokes aka receive notification from hub
connectionDeathlyHallows.on("updateDealthyHallowCount", (cloak, stone, wand) => {
    cloakSpan.innerText = cloak.toString();
    stoneSpan.innerText = stone.toString();
    wandSpan.innerText = wand.toString();
});




//invoke hub methods aka send notification to hub

//start connection
function fulfilled() {
    console.log("Connection successfull");
}
//
function rejected() {

}
connectionDeathlyHallows.start().then(fulfilled, rejected);