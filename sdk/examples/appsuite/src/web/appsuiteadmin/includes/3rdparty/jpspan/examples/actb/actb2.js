/*
Original source code found at
http://www.thecodeproject.com/jscript/jsactb.asp
used the update posted in the formus at
http://www.thecodeproject.com/jscript/jsactb.asp?df=100&forumid=92394&select=940575#xx940575xx

JES Modifications
Add additional paramerter to actb_tocomplete function to reset actb_firstText behavior
Attempt to add iframe under table to obscure IE windowing controls (like select boxes)
*/
/* ---- Variables ---- */
var actb_timeOut = 10000; // Autocomplete Timeout in ms (-1: autocomplete never time out)
var actb_lim = 10;    // Number of elements autocomplete can show (-1: no limit)
var actb_firstText = false; // should the auto complete be limited to the beginning of keyword?
var actb_same_size = true; //Should the autocomplete be the same size as the widget (true), or sized based on its elements(false)?
var actb_enable_mouse = true; //Enable mouse support, or just keyboard.  Enablingmouse with actb_timeOut set to -1 can cause some unusual behavior.
/* ---- Variables ---- */

/* ---- Constants ---- */
var actb_keywords = new Array();
var actb_display = false;
var actb_pos = 0;
var actb_total = 0;
var actb_curr = null;
var actb_rangeu = 0;
var actb_ranged = 0;
var actb_bool = new Array();
var actb_pre = 0;
var actb_toid;
var actb_tomake = false;
var mouse_on_list = 0;
/* ---- Constants ---- */

function getTargetElement(evt) {
    var elem
    if (evt.target) {
        elem = (evt.target.nodeType == 3) ? evt.target.parentNode : evt.target;
    }
    else {
        elem = evt.srcElement;
    }
    return elem;

}
function actb_parse(n,j){//added j to define span id as 'tat_td'+(j)
    var t = escape(actb_curr.value);
    var tobuild = '';
    var i;
    if (actb_firstText){
        var re = new RegExp("^" + t, "i");
    }
    else{
        var re = new RegExp(t, "i");
    }
    var p = n.search(re);
    for (i=0;i<p;i++){
        tobuild += n.substr(i,1);
    }
    tobuild += "<span class='actb_regex_match'";
    tobuild += " id='tat_td";
    tobuild += (j);
    tobuild += "' >";
    for (i=p;i<t.length+p;i++){
        for (i=p;i<t.length+p;i++){
                tobuild += n.substr(i,1);
        }
    }
    tobuild += "</span>";
    for (i=t.length+p;i<n.length;i++){
        tobuild += n.substr(i,1);
    }
    return unescape(tobuild);
}
function curTop(){
    actb_toreturn = 0;
    obj = actb_curr;
    while(obj){
        actb_toreturn += obj.offsetTop;
        obj = obj.offsetParent;
    }
    return actb_toreturn;
}
function curLeft(){
    actb_toreturn = 0;
    obj = actb_curr;
    while(obj){
        actb_toreturn += obj.offsetLeft;
        obj = obj.offsetParent;
    }
    return actb_toreturn;
}
function actb_generate(actb_bool){
   // if (document.getElementById('tat_table_div')) document.body.removeChild(document.getElementById('tat_table_div'));
   // d = document.createElement('div');
   // d.id = 'tat_table_div';
    //d.style.position = 'absolute';
    if (document.getElementById('tat_table')) document.body.removeChild(document.getElementById('tat_table'));
    a = document.createElement('table');
    a.className='actb_table';
    if (actb_same_size) a.width=field_size;
    a.style.top = eval(curTop() + actb_curr.offsetHeight) + "px";
    a.style.left = curLeft() + "px";
    a.id = 'tat_table';
    if (actb_enable_mouse){
         a.onmouseover = table_focus;
         a.onmouseout= table_unfocus;
    }
    document.body.appendChild(a);
    /*
    //document.body.appendChild(d);
    //d.appendChild(a);
    //JES added if (document.getElementById('tat_iframe_shim')) document.body.removeChild(document.getElementById('tat_iframe_shim'));
    shim = document.createElement('iframe');
    shim.className = 'iframe_shim'
    shim.id = 'tat_iframe_shim';
    shim.src = 'javascript:false;';
    shim.scrolling='no';
    shim.frameborder='0';
    d.appendChild(shim);
    //END JES added
    */
    var i;
    var first = true;
    var j = 1;
    var counter = 0;
    for (i=0;i<actb_keywords.length;i++){
        if (actb_bool[i]){
            counter++;
            r = a.insertRow(-1);
            if (first && !actb_tomake){
                r.className = 'actb_active';
                first = false;
                actb_pos = counter;
            }
            else if(actb_pre == i){
                r.className = 'actb_active';
                first = false;
                actb_pos = counter;
            }
            r.id = 'tat_tr'+(j);
            c = r.insertCell(-1);
            if (actb_enable_mouse) c.onclick = actb_click_table;
            c.innerHTML = actb_parse(escape(actb_keywords[i]),j);
            c.id = 'tat_td'+(j);
            j++;
        }
        if (j - 1 == actb_lim && j < actb_total){
            r = a.insertRow(-1);
            c = r.insertCell(-1);
            c.className='actb_arrow_down';
            if (actb_enable_mouse) c.onclick = actb_click_down;
            c.innerHTML = ' ';
            break;
        }
    }
    actb_rangeu = 1;
    actb_ranged = j-1;
    actb_display = true;
    if (actb_pos <= 0) actb_pos = 1;
    // JES added
    /*
    var DivRef = document.getElementById('tat_table_div');
	var IfrRef = document.getElementById('tat_iframe_shim');
	DivRef.style.display = "block";
	IfrRef.style.width = DivRef.offsetWidth;
	IfrRef.style.height = DivRef.offsetHeight;
	IfrRef.style.top = DivRef.style.top;
	IfrRef.style.left = DivRef.style.left;
	IfrRef.style.zIndex = DivRef.style.zIndex - 1;
    IfrRef.style.display = "block";
    alert (IfrRef.style.width  + ' ' + DivRef.offsetWidth  + ' ' +
	IfrRef.style.height  + ' ' + DivRef.offsetHeight + ' ' +
	IfrRef.style.top  + ' ' + DivRef.style.top + ' ' +
	IfrRef.style.left  + ' ' + DivRef.style.left + ' ' +
	IfrRef.style.zIndex  + ' ' + DivRef.style.zIndex - 1 + ' ' +
    IfrRef.style.display);
    /* */
    //END JES Added
}
function actb_remake(){
    document.body.removeChild(document.getElementById('tat_table'));
    a = document.createElement('table');
    a.className='actb_table';
    if (actb_same_size) a.width=field_size;
    a.style.top = eval(curTop() + actb_curr.offsetHeight) + "px";
    a.style.left = curLeft() + "px";
    a.id = 'tat_table';
    if (actb_enable_mouse){
        a.onmouseover = table_focus;
        a.onmouseout = table_unfocus;
    }
    document.body.appendChild(a);
    var i;
    var first = true;
    var j = 1;
    if (actb_rangeu > 1){
        r = a.insertRow(-1);
        c = r.insertCell(-1);
        c.className='actb_arrow_up';
        if (actb_enable_mouse) c.onclick = actb_click_up;
        c.innerHTML = ' ';
    }
    for (i=0;i<actb_keywords.length;i++){
        if (actb_bool[i]){
            if (j >= actb_rangeu && j <= actb_ranged){
                r = a.insertRow(-1);
                r.id = 'tat_tr'+(j);
                c = r.insertCell(-1);
                c.innerHTML = actb_parse(escape(actb_keywords[i]),j);
                if (actb_enable_mouse) c.onclick = actb_click_table;
                c.id = 'tat_td'+(j);
                j++;
            }
            else{
                j++;
            }
        }
        if (j > actb_ranged) break;
    }
    if (j-1 < actb_total){
        r = a.insertRow(-1);
        c = r.insertCell(-1);
        c.className='actb_arrow_down';
        if (actb_enable_mouse) c.onclick = actb_click_down;
        c.innerHTML = ' ';
    }
}
function actb_goup(){
    if (!actb_display) return;
    if (actb_pos == 1) return;
    document.getElementById('tat_tr'+actb_pos).className = '';
    actb_pos--;
    if (actb_pos < actb_rangeu) actb_moveup();
    document.getElementById('tat_tr'+actb_pos).className = 'actb_active';
    if (actb_toid) clearTimeout(actb_toid);
    if (actb_timeOut > 0) actb_toid = setTimeout("actb_removedisp(evt.srcElement)",actb_timeOut);
}
function actb_godown(){
    if (!actb_display) return;
    if (actb_pos == actb_total) return;
    document.getElementById('tat_tr'+actb_pos).className = '';
    actb_pos++;
    if (actb_pos > actb_ranged) actb_movedown();
    document.getElementById('tat_tr'+actb_pos).className = 'actb_active';
    if (actb_toid) clearTimeout(actb_toid);
    if (actb_timeOut > 0) actb_toid = setTimeout("actb_removedisp(evt.srcElement)",actb_timeOut);
}
function actb_movedown(){
    actb_rangeu++;
    actb_ranged++;
    actb_remake(field_size);
}
function actb_moveup(){
    actb_rangeu--;
    actb_ranged--;
    actb_remake(field_size);
}
function actb_click_table(evt){
    evt = (evt) ? evt : ((window.event) ? window.event : "");
    if (evt) {
        var elem = getTargetElement(evt);
        if (elem) {
                actb_pos = elem.id.substr(6,50);
                mouse_on_list = 0;
                actb_penter();
        }
    }
}

function actb_click_down(evt){
    evt = (evt) ? evt : ((window.event) ? window.event : "");
    if (evt) {
        var elem = getTargetElement(evt);
        if (elem) {
            actb_pos = elem.id.substr(6,50);
            actb_movedown();
            if (actb_toid) clearTimeout(actb_toid);
            if (actb_timeOut > 0) actb_toid = setTimeout("actb_removedisp(evt.srcElement)",actb_timeOut);
        }
    }
}

function actb_click_up(evt){
    evt = (evt) ? evt : ((window.event) ? window.event : "");
    if (evt) {
        var elem = getTargetElement(evt);
        if (elem) {
            actb_pos = elem.id.substr(6,50);
            actb_moveup();
            if (actb_toid) clearTimeout(actb_toid);
            if (actb_timeOut > 0) actb_toid = setTimeout("actb_removedisp(evt.srcElement)",actb_timeOut);
        }
    }
}

function table_focus(evt){
    mouse_on_list = 1;
}

function table_unfocus(evt){
    mouse_on_list = 0;
    if (actb_toid) clearTimeout(actb_toid);
    if (actb_timeOut > 0) actb_toid = setTimeout("actb_removedisp(evt.srcElement)",actb_timeOut);
}


function actb_penter(){
    if (!actb_display) return;
    actb_display = 0;
    var word = '';
    var c = 0;
    for (var i=0;i<=actb_keywords.length;i++){
        if (actb_bool[i]) c++;
        if (c == actb_pos){
            word = actb_keywords[i];
            break;
        }
    }
    a = word;//actb_keywords[actb_pos-1];//document.getElementById('tat_td'+actb_pos).;
    actb_curr.value = a;
    actb_removedisp();
}
function actb_removedisp(sndr){
    if (mouse_on_list == 0 || sndr.value == '') {
        actb_display = false;
        // JES edit 
	if (document.getElementById('tat_table_div')) document.body.removeChild(document.getElementById('tat_table_div'));
        if (document.getElementById('tat_table')) document.body.removeChild(document.getElementById('tat_table'));
        if (actb_toid) clearTimeout(actb_toid);
    }
}
function actb_checkkey(evt){
    a = evt.keyCode;
    if (a == 38){ // up key
        actb_goup();
    }
    else if(a == 40){ // down key
        actb_godown();
    }
    else if(a == 13){//enter key
        actb_penter();
    }
}
function actb_tocomplete(sndr,evt,arr,firstOnly){
	actb_firstText = firstOnly;
    if (arr) {
        actb_keywords = arr;
    }
    if (evt.keyCode == 38 || evt.keyCode == 40 || evt.keyCode == 13) return;
    var i;
    if (actb_display){
        var word = 0;
        var c = 0;
        for (var i=0;i<=actb_keywords.length;i++){
            if (actb_bool[i]) c++;
            if (c == actb_pos){
                word = i;
                break;
            }
        }
        actb_pre = word;//actb_pos;
    }
    else{
        actb_pre = -1;
    }
    if (!sndr) var sndr = evt.srcElement;
    actb_curr = sndr;
    field_size = sndr.offsetWidth;
    if (sndr.value == ''){
        actb_removedisp(sndr);
        return;
    }
    var t = escape(sndr.value);
    if (actb_firstText){
        var re = new RegExp("^" + t, "i");
    }
    else{
        var re = new RegExp(t, "i");
    }

    actb_total = 0;
    actb_tomake = false;
    for (i=0;i<actb_keywords.length;i++){
        actb_bool[i] = false;
        if (re.test(escape(actb_keywords[i]))){
            actb_total++;
            actb_bool[i] = true;
            if (actb_pre == i) actb_tomake = true;
        }
    }
    if (actb_toid) clearTimeout(actb_toid);
    if (actb_timeOut > 0) actb_toid = setTimeout("actb_removedisp(sndr)",actb_timeOut);
    actb_generate(actb_bool);
}

/***********************************************
* Disable "Enter" key in Form script- By Nurul Fadilah(nurul@REMOVETHISvolmedia.com)
* This notice must stay intact for use
* Visit http://www.dynamicdrive.com/ for full source code
* http://www.dynamicdrive.com/dynamicindex16/disableenter.htm
***********************************************/
function handleEnter (field, event) {
		var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
		if (keyCode == 13) {
			var i;
			for (i = 0; i < field.form.elements.length; i++)
				if (field == field.form.elements[i])
					break;
			i = (i + 1) % field.form.elements.length;
			field.form.elements[i].focus();
			return false;
		}
		else
		return true;
	}
