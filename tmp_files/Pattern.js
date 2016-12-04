var Pattern = {};
Pattern.tPattern = {};
Pattern.bPattern = {};
Pattern.bPattern.height = 200;
Pattern.tPattern.alpha = 0;
Pattern.bPattern.alpha = 0;


function drawPattern(ctx)
{
	ctx.clearRect(0, 0, w, h);	
	ctx.globalAlpha = Pattern.tPattern.alpha;
	ctx.drawImage(Pattern.tPattern.pic,-20,0);	
	ctx.globalAlpha = Pattern.bPattern.alpha;
	ctx.drawImage(Pattern.bPattern.pic,0,0);	
}

function drawTopGradient()
{
	var nCanvas = document.createElement("CANVAS");		
	nCanvas.width = w;
	nCanvas.height = h;		
	var ctx = nCanvas.getContext('2d');	
	ctx.translate(0,-120+45);
	g=ctx.createLinearGradient(0.27,201,154,238);
	g.addColorStop(0,"rgba(0, 30, 40, 1)");
	g.addColorStop(0.7,"rgba(0, 30, 40, 0.5)");
	g.addColorStop(1,"rgba(0, 30, 40, 0)");
	ctx.fillStyle = g;	
	ctx.beginPath();
	ctx.moveTo(0,240);
	ctx.lineTo(186.2,240);
	ctx.lineTo(186.2,120);
	ctx.lineTo(0,120);
	ctx.lineTo(0,240);
	ctx.closePath();
	ctx.fill();		
	g=ctx.createLinearGradient(0.27,39,154,2);
	g.addColorStop(0,"rgba(0, 30, 40, 1)");
	g.addColorStop(0.7,"rgba(0, 30, 40, 0.5)");
	g.addColorStop(1,"rgba(0, 30, 40, 0)");
	ctx.fillStyle = g;	
	ctx.beginPath();
	ctx.moveTo(186.2,120);
	ctx.lineTo(186.2,0);
	ctx.lineTo(0,0);
	ctx.lineTo(0,120);
	ctx.lineTo(186.2,120);
	ctx.closePath();
	ctx.fill();
	Pattern.topGradient = nCanvas;
}

function drawPatternElements()
{	
	drawTopGradient();
	
	var nCanvas = document.createElement("CANVAS");		
	nCanvas.width = w;
	nCanvas.height = h;		
	var ctx = nCanvas.getContext('2d');		
	ctx.drawImage(Pattern.topGradient,0,0);	
	var iPattern = ctx.createPattern(Pattern.pic, 'repeat');
	ctx.fillStyle = iPattern;
	ctx.fillRect(0,0,w,h);
	ctx.globalCompositeOperation = "source-in";	
	ctx.drawImage(Pattern.topGradient,0,0);		
	ctx.globalCompositeOperation = "source-over";	
	Pattern.tPattern.pic = nCanvas;	
	
	//////////////////////////////////////////////////////////
	var nCanvas = document.createElement("CANVAS");		
	nCanvas.width = w;
	nCanvas.height = h;	
	ctx = nCanvas.getContext('2d');			

	tpgr = ctx.createLinearGradient(parseInt(w-Pattern.bPattern.height), 0, w, 0);
	tpgr.addColorStop(0, "rgba(0, 30, 40, 0)");
	tpgr.addColorStop(0.4, "rgba(0, 30, 40, 0.6)");	
	tpgr.addColorStop(1, "rgba(0, 30, 40, 1)");		
		
	
	ctx.fillStyle=tpgr;
	ctx.fillRect(0,0,w,h);	
	
	iPattern = ctx.createPattern(Pattern.pic, 'repeat');
	ctx.fillStyle = iPattern;
	ctx.fillRect(0,0,w,h);
	ctx.globalCompositeOperation = "source-in";
	ctx.fillStyle = tpgr;
	ctx.fillRect(0,0,w,h);	
	ctx.globalCompositeOperation = "source-over";			
	Pattern.bPattern.pic = nCanvas;
	
}

