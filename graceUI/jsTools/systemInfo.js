/*
graceUI-JS - 获取系统信息并识别 iphoneX
link : graceui.hcoder.net
author : 5213606@qq.com 深海

版权声明 : 
GraceUI 的版权约束是不能转售或者将 GraceUI 直接发布到公开渠道！
侵权必究，请遵守版权约定！
*/
module.exports = {
	info : function () {
		try {
		    const res = uni.getSystemInfoSync();
			var iPhoneXBottom = 0;
			if(res.model.indexOf('iPhoneX') != -1){
				res.iPhoneXBottomHeightRpx = 72;
				res.iPhoneXBottomHeightPx = uni.upx2px(72);
			}else{
				res.iPhoneXBottomHeightRpx = 0;
				res.iPhoneXBottomHeightPx  = 0;
			}
			return res;
		} catch (e){
		    return null;
		}
	}
}