<%@ Control Language="C#" %>

<table id="tab_pasteaddress" style="display:none">
<tr><td rowspan="3">粘贴收货人地址：</td>
<td>请先选择您的代销网站类型，然后去网店复制下单用户的收货人地址，在下列大方框用"Ctrl+V"粘贴。</td>
</tr>
<tr><td>
<input type="radio" name="radaddresstype" id="radtaobao" value="taobao" checked="checked"  onclick="return PasteAddress()"/>淘宝　　
<input type="radio" name="radaddresstype" id="radpaipai" value="paipai" onclick="return PasteAddress()" />拍拍　　　
</td></tr>
<tr><td><textarea rows="5" cols="50" id="txtarea" onblur="return PasteAddress()"></textarea></td></tr>
</table>

<script>
    function PasteAddress() {
        if ($("#hdcopyshipping").val().replace(/\s/g, "") == null || $("#hdcopyshipping").val().replace(/\s/g, "") == "undefined" || $("#hdcopyshipping").val().replace(/\s/g, "") == "") {
            alert("指定参数无效！");
            return false;
        }

        if ($("#hdcopyshipping").val().replace(/\s/g, "").split(',') != null && $("#hdcopyshipping").val().replace(/\s/g, "").split(',').length != 5) {
            alert("指定参数长度有误！");
            return false;
        }

        var shippingaddress = $("#hdcopyshipping").val().replace(/\s/g, "").split(',');

        var pastetype = "";
        $("#tab_pasteaddress input[type='radio']").each(function () {
            if ($(this).attr("checked")) {
                pastetype = $(this).val();
            }
        });
        var pastecontent = $("#txtarea").val();
        var shipto = ""; //收货人
        var regioncity = ""; //城市
        var regionpro = ""; //省份
        var regionarea = ""; //区
        var ship_region_addr;
        if (pastecontent != null && pastecontent != "" && pastecontent != "undefined") {
            switch (pastetype) {
                case "taobao":
                    ship_info_arr = pastecontent.split(' ，');
                    if (ship_info_arr[0]) { $("#" + shippingaddress[0]).val(ship_info_arr[0]); $("#" + shippingaddress[0]).focus(); }
                    if (ship_info_arr[1]) $("#" + shippingaddress[1]).val(ship_info_arr[1]);
                    if (ship_info_arr[2]) { $("#" + shippingaddress[2]).val(ship_info_arr[2]); }
                    if (ship_info_arr[3]) ship_region_addr = ship_info_arr[3].split(' ');
                    if (ship_info_arr[4]) { $("#" + shippingaddress[3]).val(ship_info_arr[4]); $("#" + shippingaddress[4]).focus(); }
                    if (ship_region_addr != null && ship_region_addr != "undefined" && ship_region_addr.length >= 3) {
                        if (ship_region_addr[1]) regionpro = ship_region_addr[1];
                        if (ship_region_addr[2]) regioncity = ship_region_addr[2];
                        if (ship_region_addr[3]) regionarea = ship_region_addr[3];
                        if (ship_region_addr[4]) $("#" + shippingaddress[4]).val(ship_region_addr[4]);
                        for (var i = 4; i < ship_region_addr.length-1; i++) {
                            $("#" + shippingaddress[4]).val($("#" + shippingaddress[4]).val() + " " + ship_region_addr[i]);
                        }
                    }
                    break;
                case "paipai":
                    ship_info_arr = pastecontent.split(' ，');
                    if (ship_info_arr[0]) $("#" + shippingaddress[0]).val(ship_info_arr[0]);
                    if (ship_info_arr[1]) {
                        if (ship_info_arr[1].split('，')[0] && ship_info_arr[1].split('，')[0]) $("#" + shippingaddress[2]).val(ship_info_arr[1].split('，')[0]);
                        ship_region_addr = ship_info_arr[1].split('，')[1].split(' ');
                    }
                    if (ship_info_arr[2]) $("#" + shippingaddress[3]).val(ship_info_arr[2]);
                    if (ship_region_addr != null && ship_region_addr != "undefined" && ship_region_addr.length >= 3) {
                        if (ship_region_addr[0]) regionpro = ship_region_addr[0] + "省";
                        if (ship_region_addr[1]) regioncity = ship_region_addr[1];
                        if (ship_region_addr[2]) regionarea = ship_region_addr[2];
                        if (ship_region_addr[3]) $("#" + shippingaddress[4]).val(ship_region_addr[3]);
                        for (var i = 4; i < ship_region_addr.length; i++) {
                            $("#" + shippingaddress[4]).val($("#" + shippingaddress[4]).val() + " " + ship_region_addr[i]);
                        }
                    }
                    break;
                default:
                    break;
            };
            $.ajax({
                url: "SubmmitOrderHandler.aspx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { Action: "GetRegionId", Prov: regionpro, City: regioncity, Areas: regionarea },
                async: false,
                success: function (resultData) {
                    if (resultData.Status == "OK") {
                        ResetSelectedRegion(resultData.RegionId);
                        CalculateFreight(resultData.RegionId);
                    }
                    else {
                        alert("收货地址粘贴格式错误，请重试!");
                    }
                }
            });
        }

    }
</script>