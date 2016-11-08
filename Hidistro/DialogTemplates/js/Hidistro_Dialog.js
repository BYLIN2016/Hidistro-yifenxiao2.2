var dialog = null;
var Hidistro_Dialog = {
    dialogFixed: true, //是否静止定位
    dialogLock: true, //是否锁屏
    dialogBackGround: '#777777', //锁屏颜色
    dialogDrag: true, //是否支持拖拽
    dialogID: 'dialogwindow', // 弹出窗口ID
    dialogContent: '', //弹出窗内容
    dialoTime: 2, //时间
    dialogClose: false, //是否启动时间函数
    dialogShow: function () {
        dialog = art.dialog({
            id: Hidistro_Dialog.dialogID,
            title: "消息提示",
            lock: true,
            background: Hidistro_Dialog.dialogBackGround, // 背景色
            opacity: 0.87, // 透明度
            content: Hidistro_Dialog.dialogContent,
            fixed: Hidistro_Dialog.dialogFixed,
            drag: Hidistro_Dialog.dialogDrag,
            ok: function () {
                return SaveElmentValue();
            },
            cancel: function () {
                
            }
        });
    },
    dialogClose: function () {
        art.dialog.list[Hidistro_Dialog.dialogID].close();
    },
    dialogTitle: function (param) {   //弹出窗口标题
        art.dialog.list[Hidistro_Dialog.dialogID].title(param);
    },
    dialogTip: function (valid, textmsg) {
        art.dialog({ icon: valid, content: textmsg }).time(2);
    }
}