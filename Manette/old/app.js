function createButton(context, func,connection){
    var button = document.createElement("input");
    button.type = "button";
    button.value = "Send message";
    button.onclick = function(){
      connection.send(" message du controller");
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
			createButton(document.body, function(){
		    highlight(this.parentNode.childNodes[1]);
		    },connection);

		alert("CONNECTED");
		 
		}   
	    
	});
}


