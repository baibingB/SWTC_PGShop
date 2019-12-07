<template>
	<view>
		<!-- 表头固定 -->
		<view class="grace-data-table grace-fixed-top top" style="height:100upx; overflow:hidden;">
			<view class="left">
				<view class="title">名称</view>
			</view>
			<scroll-view class="right" :scroll-left="scLeft" @scroll="tabelScroll" scroll-x="true">
				<!-- 请修改并规划此处 width -->
				<view class="title" style="width:800upx;">
					<view>余额</view>
					<view>冻结</view>
					<view>币种</view>
					<view>备注</view>
				</view>
			</scroll-view>
		</view>
		<!-- 数据表格开始 -->
		<view class="grace-data-table" style="margin-top:100upx;">
			<!-- 左侧区域 -->
			<view class="left">
				<view class="title" v-for="(item, index) in tableData" :key="index">{{item.title}}</view>
			</view>
			<!-- 右侧侧区域 -->
			<scroll-view class="right" :scroll-left="scLeft" scroll-x="true" @scroll="tabelScroll">
				<view class="rows" style="width:800upx;" v-for="(item, index) in tableData" :key="index">
					<view class="items">{{item.value}}</view>
					<view class="items colorYellow">{{item.freezed}}</view>
					<view class="items">{{item.currency}}</view>
					<view class="items colorYellow">PG</view>
				</view>
			</scroll-view>
		</view>
	</view>
</template>
<script>
// 说明 请规划 .rows 的宽度 如内部有4个项目 每个 200 upx 则宽度 = 200 * 4 upx
// 因 uni-app 不支持动态upx 所以只能手动设置

//此数组仅用于演示循环数据
var tableScrollTimer = null;
var _self;
export default {
	data() {
		return {
			tableData : [],
			scLeft:0
		}
	},
	onLoad: function() {
				_self = this;
				_self.gRequest.get(
					_self.webUrlSys + '/exchange/balances/' + uni.getStorageSync('userid'), {},
					function(res) {
						console.log(res);
						console.log(res.data.length);
						for (let i = 0; i < res.data.length; i++) {
							let item = {
								value: res.data[i].value,
								currency: res.data[i].currency,
								freezed: res.data[i].freezed,
								title:res.data[i].title
							};
							_self.tableData.push(item);
						};
					}
				);
			},
	methods:{
		tabelScroll : function(e){
			var scLeft = e.detail.scrollLeft;
			if(tableScrollTimer != null){clearTimeout(tableScrollTimer);}
			tableScrollTimer = setTimeout(function(){
				_self.scLeft = scLeft;
			}, 50);
		}
	}
}
</script>
<style>
page{background:#F5F6F7;}
/* h5 端需要动态增加 44px [ 避开默认的头部导航 ] */
.top{top:0;}
/* #ifdef H5 */
.top{top:43px;}
/* #endif */
/* 颜色示例 */
.colorYellow{color:#F4711E !important;}
</style>