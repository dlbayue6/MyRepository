//验证
	function validTools(type,value,msg){
		var reg;
		switch(type){
			case 'isnul':
				reg = /.+$/;
				break;
			case 'chinese':
				reg = /^[\u4e00-\u9fa5]+$/i;
				break;
			case 'house':
				reg = /^(500(\.0{1,2})?|[1234]\d{2}(\.\d{1,2})?|[1-9]\d?(\.\d{1,2})?|0\.[1-9]\d?|0\.\d{1}[1-9])$/;
				break;
			case 'number':
				reg = /^\d+$/;
				break;
			case 'numword':
				reg = /^\w+$/;
				break;
			case 'email':
				reg =  /^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/;
				break;
			case 'mobile':
				reg = /^1[34578]\d{9}$/;
				break;
		}
		if (!reg.test(value)){
			wx.showModal({
				title: '提示',
				content: msg,
				mask: true
			})
			return false;
		}else{
			return true;
		}
	}

	module.exports.validTools = validTools;
