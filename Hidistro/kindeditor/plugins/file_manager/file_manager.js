var JSON_URL = '../../php/file_manager_json.php';

var KE = parent.KindEditor;
location.href.match(/\?id=([\w-]+)/i);
var id = RegExp.$1;
var fileManagerJson = (typeof KE.g[id].fileManagerJson == 'undefined') ? JSON_URL : KE.g[id].fileManagerJson;
var isAdvPositionsValue = (typeof KE.g[id].IsAdvPositions == 'undefined')?true:KE.g[id].IsAdvPositions;
var lang = KE.lang.plugins.file_manager;
KE.event.ready(function () {
    var moveupLink = KE.$('moveup', document);
    var fileCount = KE.$('fileCount', document);
    var fileCategory = KE.$('fileCategory', document);
    var viewType = KE.$('viewType', document);
    var orderType = KE.$('orderType', document);
    var listTable = KE.$('listTable', document);
    var viewTable = KE.$('viewTable', document);
    var listDiv = KE.$('listDiv', document);
    var viewDiv = KE.$('viewDiv', document);
    viewType.options[0] = new Option(lang.viewImage, 'VIEW');
    viewType.options[1] = new Option(lang.listImage, 'LIST');
    orderType.options[0] = new Option(lang.addedTime, 'UploadTime');
    orderType.options[1] = new Option(lang.addedTimeDesc, 'UploadTime desc');
    orderType.options[2] = new Option(lang.updateTime, 'LastUpdateTime');
    orderType.options[3] = new Option(lang.updateTimeDesc, 'LastUpdateTime desc');
    orderType.options[4] = new Option(lang.photoName, 'PhotoName');
    orderType.options[5] = new Option(lang.photoNameDesc, 'PhotoName desc');
    orderType.options[6] = new Option(lang.PhotoSize, 'FileSize');
    orderType.options[7] = new Option(lang.PhotoSizeDesc, 'FileSize desc');
    if (isAdvPositionsValue) {
        fileCategory.style.display = 'none';
        var span = KE.$('spanFileCate', document);
        span.style.display = 'none';
    }
    var changeType = function (type) {
        if (type == 'VIEW') {
            listDiv.style.display = 'none';
            viewDiv.style.display = '';
        } else {
            listDiv.style.display = '';
            viewDiv.style.display = 'none';
        }
    };
    var insertLink = function (url, title) {
        var stack = KE.g[id].dialogStack;
        if (stack.length > 1) {
            var parentDialog = stack[stack.length - 2];
            var dialogDoc = KE.util.getIframeDoc(parentDialog.iframe);
            KE.$('url', dialogDoc).value = url;
            KE.$('imgTitle', dialogDoc).value = title;
            var currentDialog = stack[stack.length - 1];
            currentDialog.hide();
            return true;
        } else {
            return false;
        }
    }
    var insertImage = function (url, title) {
        if (!insertLink(url, title)) {
            KE.util.insertHtml(id, '<img src="' + url + '" alt="' + title + '" border="0" />');
        }
    };
    var insertFile = function (url, title) {
        if (!insertLink(url, title)) {
            KE.util.insertHtml(id, '<a href="' + url + '" target="_blank">' + title + '</a>');
        }
    };
    var makeFileTitle = function (filename, filesize, datetime) {
        var title = filename + ' (' + Math.ceil(filesize / 1024) + 'KB, ' + datetime + ')';
        return title;
    };
    var bindTitle = function (el, data) {
        el.title = makeFileTitle(data.name, data.filesize, data.addedtime);
    };
    var bindEvent = function (el, result, data, createFunc) {
        var fileUrl = result.domain + data.path;
        fileUrl = KE.format.getUrl(fileUrl, 'absolute');
        el.onclick = (function (url, title) {
            return function () {
                insertImage(url, title);
            }
        })(fileUrl, data.name);
    };
    var createCommon = function (result, createFunc) {
        fileCount.innerText = result.total_count;
        if (!isAdvPositionsValue) {
            var categoryList = result.category_list;
            if (typeof categoryList == 'undefined') {
                fileCategory.style.display = 'none';
                var span = KE.$('spanFileCate', document);
                span.style.display = 'none';
            }
            else {

                var istemp = false;
                for (var i = 0, len = categoryList.length; i < len; i++) {
                    var data = categoryList[i];
                    sel = (result.current_cateogry == data.cId) ? true : false;
                    fileCategory.options[i] = new Option(data.cName, data.cId, sel, sel);
                    if (data.cId == "AdvertImg") {
                        istemp = true;
                    }
                }

                if (!istemp) {
                    var userAgent = window.navigator.userAgent;

                    if (userAgent.indexOf("MSIE") > 0) {
                        var sel = (result.current_cateogry == -1) ? true : false;
                        var option = document.createElement("option");
                        option.value = "-1";
                        option.innerText = lang.allCategory;
                        fileCategory.insertBefore(option, fileCategory.options[0]);
                        fileCategory.options[0].selected=sel;

                        sel = (result.current_cateogry == 0) ? true : false;
                        option = document.createElement("option");
                        option.value = "0";
                        option.innerText = lang.defaultCategory;
                        fileCategory.insertBefore(option, fileCategory.options[1]);
                        fileCategory.options[1].selected = sel;
                    }
                    else {
                        var sel = (result.current_cateogry == -1) ? true : false;
                        fileCategory.insertBefore(new Option("-1", lang.allCategory), fileCategory.options[0]);
                        fileCategory.options[0].selected = sel;
                        sel = (result.current_cateogry == 0) ? true : false;
                        fileCategory.insertBefore(new Option("0", lang.defaultCategory), fileCategory.options[1]);
                        fileCategory.options[1].selected = sel;
                    }
                }
            }
        }
        /*		if (result.current_dir_path) {
        moveupLink.onclick = function () {
        reloadPage(result.moveup_dir_path, orderType.value, createFunc);
        };
        } else {
        moveupLink.onclick = null;
        }*/
        var onchangeFunc = function () {
            changeType(viewType.value);
            if (viewType.value == 'VIEW') reloadPage(isAdvPositionsValue ? -1 : fileCategory.value, orderType.value, createView);
            else reloadPage(isAdvPositionsValue ? -1 : fileCategory.value, orderType.value, createList);
        };
        viewType.onchange = onchangeFunc;
        orderType.onchange = onchangeFunc;
        fileCategory.onchange = onchangeFunc;
    };
    var createList = function (responseText) {
        listDiv.innerHTML = '';
        var result = KE.util.parseJson(responseText);
        createCommon(result, createList);
        var table = KE.$$('table', document);
        table.className = 'file-list-table';
        table.cellPadding = 0;
        table.cellSpacing = 2;
        table.border = 0;
        listDiv.appendChild(table);
        var fileList = result.file_list;
        for (var i = 0, len = fileList.length; i < len; i++) {
            var data = fileList[i];
            var row = table.insertRow(i);
            row.onmouseover = function () { this.className = 'selected'; };
            row.onmouseout = function () { this.className = 'noselected'; };
            var cell0 = row.insertCell(0);
            cell0.className = 'name';
            var iconName = 'file-16.gif';
            var img = KE.$$('img', document);
            img.src = './images/' + iconName;
            img.width = 16;
            img.height = 16;
            img.align = 'absmiddle';
            img.alt = data.name;
            cell0.appendChild(img);
            cell0.appendChild(document.createTextNode(' ' + data.name));
            row.style.cursor = 'pointer';
            img.title = data.name;
            cell0.title = data.name;
            bindEvent(cell0, result, data, createList);
            var cell1 = row.insertCell(1);
            cell1.className = 'size';
            cell1.innerHTML = Math.ceil(data.filesize / 1024) + 'KB';
            var cell2 = row.insertCell(2);
            cell2.className = 'datetime';
            cell2.innerHTML = data.addedtime;
        }
    };
    var createView = function (responseText) {
        viewDiv.innerHTML = '';
        var result = KE.util.parseJson(responseText);
        createCommon(result, createView);
        var fileList = result.file_list;
        for (var i = 0, len = fileList.length; i < len; i++) {
            var data = fileList[i];
            var div = KE.$$('div', document);
            div.className = 'file-view-area';
            viewDiv.appendChild(div);
            var tableObj = KE.util.createTable(document);
            var table = tableObj.table;
            table.className = 'photo noselected';
            table.onmouseover = function () { this.className = 'photo selected'; };
            table.onmouseout = function () { this.className = 'photo noselected'; };
            var cell = tableObj.cell;
            cell.valign = 'middle';
            cell.align = 'center';
            var fileUrl = result.domain + data.path;
            var img = KE.$$('img', document);
            img.src = fileUrl;
            img.width = 80;
            img.height = 80;
            img.alt = data.ame;
            table.style.cursor = 'pointer';
            bindTitle(img, data);
            bindTitle(table, data);
            bindEvent(table, result, data, createView);
            cell.appendChild(img);
            div.appendChild(table);
            var titleDiv = KE.$$('div', document);
            titleDiv.className = 'name';
            titleDiv.title = data.name;
            titleDiv.innerHTML = data.name;
            div.appendChild(titleDiv);
        }
    };
    var httpRequest = function (param, func) {
        KE.util.showLoadingPage(id);
        var req = window.ActiveXObject ? new ActiveXObject("Microsoft.XMLHTTP") : new XMLHttpRequest();
        var url = fileManagerJson;
        url += param;
        url += (url.match(/\?/) ? "&" : "?") + (new Date()).getTime()
        req.open('GET', url, true);
        req.onreadystatechange = function () {
            if (req.readyState == 4) {
                if (req.status == 200) {
                    func(req.responseText);
                    KE.util.pluginLang('file_manager', document);
                    KE.util.hideLoadingPage(id);
                }
            }
        };
        req.send(null);
    };
    var reloadPage = function (cid, order, func) {
        httpRequest('?cid=' + cid + '&order=' + order + '&isAdvPositions=' + isAdvPositionsValue, func);
    };
    changeType('VIEW');
    viewType.value = 'VIEW';
    reloadPage('-1', orderType.value, createView);
}, window, document);