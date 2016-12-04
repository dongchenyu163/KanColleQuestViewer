var font = "'ヒラギノ角ゴ Pro W3', 'Hiragino Kaku Gothic Pro', Osaka, 'メイリオ', Meiryo, 'ＭＳ Ｐゴシック', 'MS PGothic', sans-serif";

var againText  = "再挑戦";

var icoText    = 3;

var tpText1    = "敵艦艇を撃沈せよ！";

var tpText2_1  = "見事な砲撃だ！";
var tpText2_2  = "さあ 本当の戦闘を始めよう！";

var tpText3_1  = "敵艦艇を撃破できなかった";
var tpText3_2  = "砲撃スキルを磨こう！";

////////////////////////////////////////////////////
//
function drawIcoText(ctx)
{
	ctx.font = 'bold 12pt '+ font;	
	ctx.textBaseline = "middle";
	ctx.textAlign = "center";
	//
	ctx.shadowColor = "rgb(255, 255, 255)";
	ctx.shadowBlur = 2;
	ctx.shadowOffsetX = 0;
	ctx.shadowOffsetY = 0;
	//
	ctx.fillStyle = "rgb(95, 115, 125)";
	//
	ctx.fillText(icoText, 28, 64);
}

//
function drawTPText(ctx, txtNum)
{
	if(txtNum === 1)
	{
		ctx.font = '11pt ' + font;
		ctx.fillText(tpText1, 0, 0);
	}
	else if(txtNum === 2)
	{
		ctx.font = '11pt ' + font;
		ctx.fillText(tpText2_1 + " " + tpText2_2, 0, 0);
	}
	else
	{
		ctx.font = '11pt ' + font;
		ctx.fillText(tpText3_1 + " " + tpText3_2, 0, 0);
	}
}