
//////  VARIABLES GLOBALES ///////////////
var isConnected =0;



//////  FONCTIONS ///////////////////////

function setVisible(idName)
{
  document.getElementById(idName).style.display = 'block';
}

function setHidden(idName)
{
  document.getElementById(idName).style.display = 'none';
}
 

function createButton(context, func,connection){
    var button = document.createElement("input");
    button.type = "button";
    button.value = "Send message";
    button.onclick = function(){
      connection.send(" message blabla");
    };
    context.appendChild(button);
}

function connexion()
{
	$("#connect").click(function(event)
	{
		console.log($("#url").val());

		var url = "ws://"+$("#url").val();
		var connection = new WebSocket(url);
		console.log(connection);
		
		connection.onopen = function (event) 
		{
			/*createButton(document.body, function(){
		    highlight(this.parentNode.childNodes[1]);
		    },connection);*/

		setConnected();
		alert("CONNECTED");
        //if connected, display controllerPage and hide connexion page
        setHidden("connectionPage");
        setVisible("controllerPage");
        
   
		 
		}   
	    
	});
}

function setConnected()
{
  isConnected=1;
}
/////// LISTENER /////////////////////////
/*
//disable scroll
window.addEventListener("scroll",noscroll);

function noscroll(){
	window.scrollTo(0,0);
}
*/


//ultime no scroll
document.ontouchmove = function(event){
    event.preventDefault();
}


//hide menu bar
window.addEventListener("load",function() {
	// Set a timeout...
	setTimeout(function(){
		// Hide the address bar!
		window.scrollTo(0, 1);
	}, 0);
});

window.addEventListener("orientationchange", function(e) {       
    
    if (window.matchMedia("(orientation: portrait)").matches) {
        //portrait mode
        setVisible("portrait");
        setHidden("landscape");
    }
    else
    {
        //landscape mode
        setVisible("landscape");
        setHidden("portrait");

        if (isConnected == 0)
            {
           
                setVisible("connectionPage");
                setHidden("controllerPage");
                connexion();         
            }    

    }


        
        
});

/*
document.addEventListener('gesturestart', function (e) {
    e.preventDefault();
});
*/
///// MAIN //////////////////

if (window.matchMedia("(orientation: portrait)").matches) {
   //portrait mode
    setVisible("portrait");
    setHidden("landscape");
}
else
{
    //landscape mode
    setVisible("landscape");
    setHidden("portrait");

    
    
    if (isConnected == 0)
    {
        setVisible("connectionPage");
        setHidden("controllerPage");

        //if it isn't connect, display form to register to the server      
        connexion();      
    }
}

