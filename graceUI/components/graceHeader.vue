<template>
	<view>
		<view class="grace-header" :style="{background:background, height:height+'rpx', 'padding-top':top+'px'}">
			<view class="grace-header-main"><slot></slot></view>
			<view :style="{width:BoundingWidth, height:'10px'}"></view>
		</view>
		<!-- 占位 view -->
		<view :style="{height:paddingTop+'px'}" v-if="isSeize"></view>
	</view>
</template>
<script>
export default {
	props: {
		background:{
			type : String,
			default : "#F8F8F8"
		},
		height:{
			type : Number,
			default : 90
		},
		haveStatusBar:{
			type : Boolean,
			default : true
		},
		isSeize : {
			type:Boolean,
			default : true
		}
	},
	data(){
		return{
			top : 0,
			paddingTop : 0,
			BoundingWidth :'0px'
		}
	},
	created:function(){
		if(!this.haveStatusBar){return ;}
		var res  = uni.getSystemInfoSync();
		this.top = res.statusBarHeight;
		this.paddingTop = this.top + uni.upx2px(this.height); 
		// #ifdef MP
		// 小程序胶囊按钮
		var res  = wx.getMenuButtonBoundingClientRect();
		this.BoundingWidth = (res.width + 20) + 'px';
		// #endif
	}
}
</script>
<style>
.grace-header{width:100%; position:fixed; left:0; top:0; z-index:99; height:44px; padding-top:20px; display:flex; flex-direction:row; flex-wrap:nowrap;}
.grace-header-main{width:300rpx; flex:auto;}
</style>