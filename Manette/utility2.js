$(document).ready(function () {


//////  VARIABLES GLOBALES ///////////////
var isConnected =0;
var connexionKey=0;
var showConnexionScreen=1;//l'ecran de connexion doit s'afficher au demarrage
var url='';
var name='';
var i=0;
var messageArray = new Array();
var clonedMessageScreen =  $('#MessageScreen').clone();
var clonedInfoObject =  $('#infoObject').clone();
var isObjectInInventory=0;
var pathImageObject="";
var nameObject="";
var descriptionObject="";


////// LISTENER //////////////////////////

/*
$(document).on("keydown", function(evt) {
	if(evt.key == "m")
	{
		pressMenu();
	}	
});

$(document).on("keydown", function(evt) {
	if(evt.key == "i")
	{
		pressInventory();
	}	
});

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
	if(evt.key== "c")
	{
		pressClose();
	}
	if(evt.key== "l")
	{
		pressMenuMessages();
	}
	if(evt.key=="v")
	{
		pressCloseInventory();
	}
	if(evt.key=="t")
	{
		pressThrow();
	}
	if(evt.key=="w")
	{
		pressCloseSocket();
	}
	if(evt.key=="x")
	{
		pressExitLevel();
	}
	

});
*/
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

$("#button_close").on('touchstart', pressClose);
$("#button_menu").on('touchstart', pressMenu);
$("#button_inventory").on('touchstart', pressInventory);
$("#button_messages").on('touchstart', pressMenuMessages);
$("#button_throw").on('touchstart', pressThrow);
$("#button_closeInventory").on('touchstart', pressCloseInventory);
//$("#button_reconnexion").on('touchstart', pressReconnexion);
//$("#button_configuration").on('touchstart', pressconfiguration);
$("#button_exitLevel").on('touchstart', pressExitLevel);

$("#button_closeSocket").on('touchstart', pressCloseSocket);

function pressExitLevel()
{
	console.log("button_exitLevel pressed"); 
	sendMsg ("112", "");
	pressClose();
	

}


function deleteCookie(name)
{
	document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}


function pressCloseSocket()
{
	console.log("button_closeSocket pressed"); 
	sendMsg ("102", "");
	//socket detruite, lorsque la page est recharge
	//supprime les cookies
	deleteCookie("smartEnigmaUrl");
	deleteCookie("smartEnigmaName");
	deleteCookie("smartEnigmaConnexionKey");

	
		setVisible("connexionScreen");
		setHidden("interfaceScreen");
		isConnected = 0;

	/*
	document.location.href="index.html";
	reconnect();
*/
}



function reconnect()
{
	console.log("passe reconnnect");
	//remet les champ du submit enregistre	
	
	if ((url!="") && (name != ""))
	{
		//document.getElementById("url").value = url;
		//document.getElementById("name").value = name;
		console.log("url "+url);
		console.log("name"+name);
	}	

	

	$("#connect").click(function(event)
	{
		var urlGiven2 = "ws://"+document.getElementById("url").value;
		updateUrl(document.getElementById("url").value);

		var name2 = document.getElementById("name").value;  
		var isSameName=1;
		console.log("cookieName* : "+cookieName);
		if(name2!=cookieName)
		{
			isSameName=0;
		}
		updateName(name2);  


		//creation de la socket
        setSocket(new WebSocket(urlGiven2));
		
		console.log(socket);

		socket.onopen = function (event) 
		{
			/*
			if(isSameName)
			{
				var tempVar=getCookie("smartEnigmaConnexionKey");
				console.log("tentative de reconnexion avec la cle"+tempVar);
				socket.send("101|"+tempVar);				
			}
			else*/
			{
				//console.log("joueur a change de nom");
				//nouveau joueur, sur le meme telephone
				socket.send("100|"+name2);
				
			}			
     		
			setConnected();

			//attente des messages
			waitMsg();

			//declenchement du listener qui agit en cas de deconnexion au serveur
			listenerSendMessageWhenDisconnect();

			console.log("CONNECTED");   
		 
		}

	});

	
	
}



function pressCloseInventory()
{
	console.log("button_closeInventory pressed");   
	$("#inventoryScreen").slideUp();
}

function pressThrow(){
	console.log("button_throw pressed");  
	//il n'y a plus d'objet dans l'inventaire
	updateIsObjectInInventory(0);

    sendMsg ("120", "");

    //l'item est jete de l'inventaire, inventaire est vide
    $("#inventoryScreen").slideUp();

}

function pressClose() {
    console.log("button_close pressed");       		
    $("#menuScreen").slideUp();
    
    //timeout ,
    setTimeout(function(){     	
    	//setHidden("ConfigurationScreen");
    	setHidden("noMessage");
    	sethidden("MessageScreen");    	
    	
    }, 500);
        	
}

function pressMenu() {
    console.log("button_menu pressed");   
    setHidden("MessageScreen");
    $("#menuScreen").slideDown();

        	
}

function pressInventory() {
    console.log("button_inventory pressed");

    if(isObjectInInventory)
    {
    	console.log("affichage inventaire non vide. valeur de isObjectInInventory : "+isObjectInInventory);
    	setVisible("objectScreen");
    	setHidden("noObjectScreen");

    	$('#infoObject').replaceWith(clonedInfoObject);
	
		//reinitialisation de la valeur des champs
		$("#imageObject").text('');
		$("#NameObject").text('');
		$("#descriptionObject").text('');

	   	
	    //image
	    var img = document.createElement("IMG");
	    //img.src = "images/gold.png";
	    img.src = pathImageObject+"";
	    img.setAttribute("width","80");
	    img.setAttribute("height","70");
	    document.getElementById('imageObject').appendChild(img);
	   
	
	   //nom
	   //$("#NameObject").text('gold');
	   $("#NameObject").text( nameObject);

	   //description
		//$("#descriptionObject").text('pour acheter des choses');
		$("#descriptionObject").text(descriptionObject);
   	
    }
    else
    {
    	setVisible("noObjectScreen");
    	setHidden("objectScreen");

    }


    //affichage
    $("#inventoryScreen").slideDown();

    
        	
}

function pressMenuMessages(){

	console.log("button_messages pressed");
	
	//recuperation du div vide

	if(messageArray.length!=0)
	{
		$('#MessageScreen').replaceWith(clonedMessageScreen);
		$("#listMessages").text('');

		//var messageArray = ["test 1", "test 2", "test 3", "test 4", "test 5"];
		var idMsg = [];
		for (var i = 0; i <messageArray.length; i++) {
		     	console.log(messageArray[i]);

		     	//create new div
		     	var textMsg = document.createElement('button');
		     	var idTextMsg = guidGenerator();
				idMsg.push(idTextMsg);
				$(textMsg).hide();

		     	textMsg.appendChild(document.createTextNode(messageArray[i]));    	
		     	textMsg.id = idTextMsg;     	
		     	
		    	document.getElementById('listMessages').appendChild(textMsg); 
		}

		var page=1;
		var maxpage = Math.floor((messageArray.length+1)/2);

		var visibleElements = [0, 1];
		$(document.getElementById(idMsg[visibleElements[0]])).show();
		$(document.getElementById(idMsg[visibleElements[1]])).show();
		


		$("#next").on('touchstart',function(){
			if (page < maxpage ){
				$(document.getElementById(idMsg[visibleElements[0]])).hide();
				$(document.getElementById(idMsg[visibleElements[1]])).hide();
				visibleElements = [visibleElements[0]+2,visibleElements[1]+2];   
				$(document.getElementById(idMsg[visibleElements[0]])).show();
				if (document.getElementById(idMsg[visibleElements[1]])){
					$(document.getElementById(idMsg[visibleElements[1]])).show();
				}
				page++;
		  	}
		});

		$("#previous").on('touchstart',function(){
			if (page > 1 ){ 
				$(document.getElementById(idMsg[visibleElements[0]])).hide();
				if (document.getElementById(idMsg[visibleElements[1]])){
					$(document.getElementById(idMsg[visibleElements[1]])).hide();
				}
				visibleElements = [visibleElements[0]-2,visibleElements[1]-2];   
				$(document.getElementById(idMsg[visibleElements[0]])).show();
				$(document.getElementById(idMsg[visibleElements[1]])).show();
				page--;
			}
		});
		$("#MessageScreen").slideDown();  
	}
	else
	{
		$("#noMessage").slideDown();  
	}
		
}


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

function updateObjectData(idObject,name,description)
{

	pathImageObject="images/items/"+idObject+".png";
	nameObject=name;
	descriptionObject=description;
}

//met a jour isObjectInInventory (1: il y a un objet dans l'inventaire)
function updateIsObjectInInventory(newVal)
{
	isObjectInInventory=newVal;
}

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

function updateName(newVal)
{
	name=newVal;
}

//renvoie un message au serveur quand le client est deconnecte
function listenerSendMessageWhenDisconnect()
{
	socket.onclose = function (e) {

		//renvoie a la page de connexion (mais les champ sont deja saisies)
		//alert("deconnexion du serveur ! ");
/*
		document.location.href="index.html";
		reconnect();
	*/
		setVisible("connexionScreen");
		setHidden("interfaceScreen");
		isConnected = 0;
		/*
		createReconnexionButton(document.body, function(){
		    highlight(this.parentNode.childNodes[1]);
		    });	 
		*/
		   
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
		//alert("enregistrement du cookie :"+connexionKey);
	}
	

}


function monitorStack() {

	$('#message-popup').innerHTML = // tete de file
	//bordel animation

	// si file pas vide
	setTimeout(monitorStack, 4000);
}

//gere les actions a effectuer en fonction du message reçu

function handleMessage (message) {	

	//recherche du messageId
	var regex = /(.{3})\|(.*)/;
	//var regex = /(.{3})\|(.{20}|\\d)/;//le message contient soit 20 caractere soit un chiffre.
	//regex.test(message);

	var match = regex.exec(message);
	
	var messageId = match[1];
	console.log("messageId : "+ messageId);

	var messageContent=null;

	if (match[2]!=null)
	{
		var messageContent = match[2];
		console.log("messagecontent : "+messageContent );
	}
	

	//different traitement selon messageId
	switch(messageId) {

    case "200":
        //Serveur a envoyé message pour indiquer que le joueur a bien ete cree. Fournit une connexionKey
        console.log("[201|1] [INFO] Enregistrement du connexionId.");

        updateConnexionKey(messageContent);

        //test de recuperation du cookie
        var tempVar=getCookie("smartEnigmaConnexionKey");
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
	        console.log("[201|_] [ERROR] Code erreur non reconnu");
		}

		break;

	case "202":
        //Serveur a envoyé message pour indiquer que le joueur a bien ete cree. Fournit une connexionKey
        console.log("[202] [ERROR] Le serveur est etteint");

        
        break;

	case "210":
        //Serveur a envoyé message pour indiquer que le joueur a bien ete cree. Fournit une connexionKey
        //alert("[210|...] [INFO] Message provenant d'un objet du jeu + "+messageContent);

        
        

        console.log("msg recu"+messageContent);
		var regexMessage = /(.*);(.*)/;
	

		var match = regexMessage.exec(messageContent);
	
		var senderName = match[1];
		var contentData = match[2];	

		console.log("messageId : "+ senderName);
		console.log("messagecontent : "+contentData );

		var msgToAdd= '['+senderName+'] '+contentData;

		//ajout du message dans la pile de message, s'il n'est pas deja present
		if(jQuery.inArray(msgToAdd, messageArray) == -1)
		{
			messageArray.unshift(msgToAdd);
			console.log("msg ajoutee : "+msgToAdd);

		}
		

		var textPopup = document.createElement("div");
		var idTextPopup = guidGenerator();
    	textPopup.appendChild(document.createTextNode(msgToAdd));
    	textPopup.id = idTextPopup;
   		document.getElementById('message-popup').appendChild(textPopup);
   		setHidden(idTextPopup);
				
	    //$("#message-popup").text(msgToAdd);
		$("#message-popup").fadeIn({ duration : 1000, start : function(){
			setVisible(idTextPopup);
			console.log('start');
		}});

	    $("#message-popup").fadeOut(3000, function(){
	    	remove(idTextPopup);//detruire
	    });
			   
      

        break;

    case "220":
        //Serveur a envoyé message pour indiquer que le joueur a bien ete cree. Fournit une connexionKey
        console.log("[202] : msg recu"+messageContent);
		var regexObject = /(.*);(.*);(.*)/;

		var matchObject = regexObject.exec(messageContent);
	
		var idObject = matchObject[1];
		var descriptionObject = matchObject[2];
		var nameObject = matchObject[3];

		console.log("idobject : "+ idObject);
		console.log("descriptionObject : "+ descriptionObject);
		console.log("nameObject : "+ nameObject);

		if(idObject == 0)
		{
			//plus d'objet dans l'inventaire
			updateIsObjectInInventory(0);
		}
		else
		{
			//ajout d'un objet dans l'inventaire
			
			updateObjectData(idObject,nameObject,descriptionObject);
			updateIsObjectInInventory(1);
		}
		
		

      

        break;
       
    default:
        console.log("["+messageId+"|_] [ERROR] ID du message non reconnu : "+messageId);
	}


}



function remove(id) {
    var elem = document.getElementById(id);
    return elem.parentNode.removeChild(elem);
}


function guidGenerator() {
    var S4 = function() {
       return (((1+Math.random())*0x10000)|0).toString(16).substring(1);
    };
    return (S4()+S4()+"-"+S4()+"-"+S4()+"-"+S4()+"-"+S4()+S4()+S4());
}




//reception des messages
function waitMsg (){

	socket.addEventListener('message', function (event) {    
		/*
		var reader = new FileReader();
		var msgString = event.data;
		reader.onload = function() {    
			handleMessage(reader.result);
		}
		reader.readAsText(event.data);	
		*/
		handleMessage(event.data);
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


function connexion()
{
	$("#connect").click(function(event)
	{

		//console.log($("#url").val());

		//var urlGiven = "ws://"+$("#url").val();
		var urlGiven = "ws://"+document.getElementById("url").value;
		updateUrl(document.getElementById("url").value);
		setCookie("smartEnigmaUrl", document.getElementById("url").value, 1);

		//var name=$("#name").val();  
		var name = document.getElementById("name").value;  
		updateName(name);    
		setCookie("smartEnigmaName", name, 1);
        
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

			console.log("CONNECTED");   
		 
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

var cookieUrl=getCookie("smartEnigmaUrl");
var cookieName=getCookie("smartEnigmaName");

if ((cookieUrl!="") && (cookieName!=""))
	{
		document.getElementById("url").value = cookieUrl;
		document.getElementById("name").value = cookieName;
		
}

if (isConnected == 0)
{	
	setVisible("connexionScreen");
	setHidden("interfaceScreen");

	if ((cookieUrl!="") && (cookieName!=""))
	{		
		reconnect();
	}
	else
	{

		connexion();      
	}
	           
}
else
{	
	setHidden("connexionScreen");
	setVisible("interfaceScreen");

}
});