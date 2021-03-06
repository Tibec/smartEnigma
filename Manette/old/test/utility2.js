$(document).ready(function () {


//////  VARIABLES GLOBALES ///////////////
var isConnected =0;
var connexionKey=0;
var showConnexionScreen=1;//l'ecran de connexion doit s'afficher au demarrage
var url='';
var i=0;


////// LISTENER //////////////////////////

$(document).on("keydown", function(evt) {
	if(evt.key == " ")
	{
		pressB();
	}	
	if(evt.key == "a")
	{
		pressA();
	}	
	if(evt.key == "z")
	{
		sendMsg ("110", "3;90;10");
	}	
	if(evt.key == "d")
	{
		sendMsg ("110", "3;0;10");
	}	
	if(evt.key == "q")
	{
		sendMsg ("110", "3;180;10");
	}	
	if(evt.key == "s")
	{
		sendMsg ("110", "3;270;10");
	}	
});

$(document).on("keyup", function(evt) {
	if(evt.key == " ")
	{
		releaseB();
	}	
	if(evt.key == "a")
	{
		releaseA();
	}	
	if(evt.key == "z" || evt.key == "q" || evt.key == "s" || evt.key == "d")
	{
		sendMsg ("111", "3");
	}	
});


$("#button-a").on('touchstart', pressA);
$("#button-a").on('touchend', releaseA);

$("#button-b").on('touchstart', pressB);
$("#button-b").on('touchend', releaseB);

function pressA() {
       		console.log("BtnA:Pressed");
        	sendMsg ("110", "1;0;0");
}

function releaseA() {
       		console.log("BtnA:Released");
        	sendMsg ("111", "1");
}

function pressB() {
       		console.log("BtnB:Pressed");
        	sendMsg ("110", "2;0;0");
}
function releaseB() {
       		console.log("BtnB:Released");
        	sendMsg ("111", "2");
}




var joystick = new VirtualJoystick({
		mouseSupport	: true,
		stationaryBase	: true,
		baseX		: 0,
		baseY		: 0,
		//limitStickTravel: true,
		stickRadius: 150, 
		container : document.getElementById("joystick-item"), 
		strokeStyle	: 'white' 
	});
	/*
	setInterval(function(){
		var outputEl	= document.getElementById('result');
		outputEl.innerHTML	= '<b>Result:</b> '
			+ ' dx:'+joystick.deltaX()
			+ ' dy:'+joystick.deltaY()
			+ (joystick.right()	? ' right'	: '')
			+ (joystick.up()	? ' up'		: '')
			+ (joystick.left()	? ' left'	: '')
			+ (joystick.down()	? ' down' 	: '')	
	}, 1/30 * 1000);	
	*/
//listener qui detecte les evenements du joystick

$(joystick._container).on("moved", function(){
	console.log("joystick bouger");
	var dx = joystick.deltaX();
	var dy = joystick.deltaY();
	
	var angle = (Math.atan2(-dy,dx) * 180/Math.PI + 360) % 360;
	var force = Math.sqrt( (dx * dx) + (dy * dy) );
	angle = Math.round(angle);
	force = Math.round(force);
	
	sendMsg ("110", "3;"+angle+";"+force);

});

$(joystick._container).on("released", function(){
	console.log("joystick laché");
	sendMsg ("111", "3");

});
/*
joystick.on('end move', function (evt, data) {

            if (evt.type === 'move'){

            	//envoie des nouvelles valeurs au serveur
            	//envoie de l'angle et de la force

            	var intDegree = Math.round(data.angle.degree);
            	var intForce = Math.round(data.force);

                console.log("valeur de l'angle : "+intDegree);
                console.log("valeur de la force : "+intForce);
                sendMsg ("110", "3;"+intDegree+";"+intForce);

            }
            else if(evt.type === 'end'){
                console.log("le joystick a ete lache");

                //previent le serveur que le joystick a ete lache
                sendMsg ("111", "3");

            }

            return false;
   
        });
*/
//joystick.off('event', handler);

//no scroll


document.body.addEventListener('touchmove', function(event) {
  event.preventDefault();
}, false); 




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

//////  FONCTIONS ///////////////////////

//met a jour showConnexionScreen (1 : ecran doit s'afficher. 0 : ecran ne doit pas s'afficher)


function updateShowConnexionScreen(newVal)
{
	showConnexionScreen=newVal
	if(showConnexionScreen)
	{
		//montre ecran de connexion
		setVisible("connexionScreen");
		setHidden("interfaceScreen");
	}
	else
	{
		//cache ecran de connexion
		setHidden("connexionScreen");
		setVisible("connexionScreen");
	}
}

//fait apparaitre div
function setVisible(idName)
{	
  	document.getElementById(idName).style.display = 'block';
}

//cache div
function setHidden(idName)
{	
  	document.getElementById(idName).style.display = 'none';
}


//creation d'un cookie
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays*24*60*60*1000));
    var expires = "expires="+ d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}




//recuperation de la valeur d'un cookie
function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for(var i = 0; i <ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

//met a jour la valeur de l'url
function updateUrl( newVal)
{
	url= newVal;
}


//renvoie un message au serveur quand le client est deconnecte
function listenerSendMessageWhenDisconnect()
{
	socket.onclose = function (e) {
		alert("deconnexion du serveur ! ");
		
		createReconnexionButton(document.body, function(){
		    highlight(this.parentNode.childNodes[1]);
		    });	 
		   
	}; 
}


//met a jour la valeur de connexionKey
function updateConnexionKey( newVal)
{
	//mise a jour de la valeur du cookie (si elle n'est pas vide)
	if(newVal)
	{
		connexionKey=newVal;
		//sauvegarde de la cle de connexion dans un cookie
		//document.cookie = "smartEnigmaConnexionKey="+newVal; 
		setCookie("smartEnigmaConnexionKey", newVal, 1);
	}
	

}


//gere les actions a effectuer en fonction du message reçu

function handleMessage (message) {	

	//recherche du messageId
	var regex = /(.{3})\|(.*)/;
	//var regex = /(.{3})\|(.{20}|\\d)/;//le message contient soit 20 caractere soit un chiffre.
	//regex.test(message);

	var match = regex.exec(message);
	
	var messageId = match[1];
	var messageContent = match[2];	

	console.log("messageId : "+ messageId);
	console.log("messagecontent : "+messageContent );

	//different traitement selon messageId
	switch(messageId) {

    case "200":
        //Serveur a envoyé message pour indiquer que le joueur a bien ete cree. Fournit une connexionKey
        console.log("[201|1] [INFO] Enregistrement du connexionId.");

        updateConnexionKey(messageContent);

        //test de recuperation du cookie
        var tempVar=getCookie("smartEnigmaConnexionKey");
        alert("test de recuperation du cookie : "+tempVar);
        console.log("test de recuperation du cookie : "+tempVar);
        break;

    case "201":
        //Le serveur a renvoyé un message d'erreur.
        //la valeur precise le message d'erreur
        switch(messageContent) {

	    case "1":
	        //serveur plein
	        console.log("[201|1] [ERROR] Serveur plein ! ");
	        break;

	    case "2":
	        //partie en cours
	        console.log("[201|2] [ERROR] Partie en cours ! ");
	        break;

	    case "3":
	        //LoginDejaUtilise
	        console.log("[201|3] [ERROR] Login deja utilise ! ");
	        break;

	    case "4":
	        //LoginDejaUtilise
	        console.log("[201|4] [ERROR] Cle invalide ! ");
	        break;


	    default:
	    	//le code erreur saisi n'est pas connu
	        console.log("[201|_] [ERROR] Code erreur non reconnu : "+messageContent);
		}

	case "210":
        //Serveur a envoyé message pour indiquer que le joueur a bien ete cree. Fournit une connexionKey
        console.log("[210|...] [INFO] Message provenant d'un objet du jeu + "+messageContent);
        updateConnexionKey(messageContent);
        break;
       
    default:
        console.log("["+messageId+"|_] [ERROR] ID du message non reconnu : "+messageId);
	}


}


//reception des messages
function waitMsg (){

	socket.addEventListener('message', function (event) {    

		var reader = new FileReader();
		reader.onload = function() {    
			handleMessage(reader.result);
		}
		reader.readAsText(event.data);	
	});
}



//envoie des messages
function sendMsg (id, content){
	
	socket.send(id+"|"+content);
}

 
function setSocket(sock)
{
    socket=sock;
}


function createReconnexionButton(context, func)
{
	var button = document.createElement("input");
    button.type = "button";
    button.value = "reconnexion";
    button.onclick = function(){

    	//creation d'une nouvelle socket
    	setSocket(new WebSocket(url));
    	socket.onopen = function (event) 
		{
			createButton(document.body, function(){
		    highlight(this.parentNode.childNodes[1]);
		    });

		
		//attente des messages
		waitMsg();

		//declenchement du listener qui agit en cas de deconnexion au serveur
		listenerSendMessageWhenDisconnect();        

        //recuperation de la cle de connexion en cookie
        var tempVar=getCookie("smartEnigmaConnexionKey");
        socket.send("101|"+tempVar);

		alert("RECONNECTED");   
		 
		}     	 
    };
    context.appendChild(button);
}

function connexion()
{
	$("#connect").click(function(event)
	{

		console.log($("#url").val());

		var urlGiven = "ws://"+$("#url").val();
		updateUrl(urlGiven);

		var name=$("#name").val();        
        
        //creation de la socket
        setSocket(new WebSocket(urlGiven));
		
		console.log(socket);
		
		socket.onopen = function (event) 
		{

			socket.send("100|"+name);
     		
			setConnected();

			//attente des messages
			waitMsg();

			//declenchement du listener qui agit en cas de deconnexion au serveur
			listenerSendMessageWhenDisconnect();

			alert("CONNECTED");   
		 
		}   
	    
	});
}

function setConnected()
{
  	isConnected=1;
  	setHidden("connexionScreen");
  	setVisible("interfaceScreen");
}


///// MAIN //////////////////


if (isConnected == 0)
{	
	setVisible("connexionScreen");
	setHidden("interfaceScreen");
	connexion();      
           
}
else
{	
	setHidden("connexionScreen");
	setVisible("interfaceScreen");

}   

});
