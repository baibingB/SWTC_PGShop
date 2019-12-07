<template>
	<view>
		<!-- 头部导航及搜索条 -->
		<graceHeader background="#FF0036">
			<view class="grace-header-body">
				<!-- 中间内容 -->
				<view class="grace-search">
					<view class="grace-search-in">
						<view class="icons grace-icons icon-search"></view>
						<view class="input"><input type="search" placeholder="程序小哥哥赶工中..." /></view>
					</view>
				</view>
			</view>
		</graceHeader>
		<!-- 轮播图 start -->
		<swiper class="grace-swiper swiper1" autoplay="true" indicator-dots indicator-color="rgba(255, 255, 255, 1)"
		 indicator-active-color="#FF0036" style="height:288rpx" interval="3000">
			<swiper-item v-for="(item, index) in swiperItems" :key="index">
				<navigator url='' class="grace-img-in">
					<image :src="item" mode="widthFix"></image>
				</navigator>
			</swiper-item>
		</swiper>
		<!-- 轮播图 end -->
		<view class="common-line"></view>
		<!-- 推荐图标 start -->
		<view class="grace-grids">
			<navigator @tap="openWebUrl1" class="items">
				<view class="icon">
					<image src="../../static/wl.jpg" mode="widthFix"></image>
				</view>
				<view class="text">斯威特联盟</view>
			</navigator>
			<navigator @tap="openWebUrl2" class="items">
				<view class="icon">
					<image src="../../static/shop2.jpg" mode="widthFix"></image>
				</view>
				<view class="text">书签购物</view>
			</navigator>
			<navigator class="items">
				<view class="icon">
					<image src="../../static/shop3.png" mode="widthFix"></image>
				</view>
				<view class="text">必然好货</view>
			</navigator>
			<navigator @tap="openWebUrl3" class="items">
				<view class="icon">
					<image src="../../static/qt.png" mode="widthFix"></image>
				</view>
				<view class="text">威链</view>
			</navigator>
			<navigator class="items">
				<view class="icon">
					<image src="../../static/shop4.jpg" mode="widthFix"></image>
				</view>
				<view class="text">推荐链接</view>
			</navigator>
		</view>
		<!-- 推荐图标 end -->
		<view class="common-line"></view>
		<!-- 猜你喜欢 start -->
		<view class="grace-title grace-margin">
			<view>PG产品</view>
			<!-- 换一批调用 youlike 函数，利用后端随机更新喜欢的内容即可 -->
			<view class="grace-more">
				<text class="grace-icons icon-refresh grace-black9" style="margin-right:8px;"></text>精品展示
			</view>
		</view>
		<view class="grace-margin">
			<view class="grace-img-card">
				<navigator class="item" @tap="productInfo" v-for="(item, index) in youlikes" :id="item.FGDSEQ" :key="item.FGDSEQ">
					<view class="img">
						<image :src="item.FURL" mode="widthFix"></image>
					</view>
					<view class="title grace-ellipsis">{{item.FGDNAME}}</view>
					<view class="more">
						PG {{item.FZKJJ}} <text class="del">￥{{item.FHSJJ}}</text>
					</view>
				</navigator>
			</view>
			<graceLoading :loadingType="loadingType"></graceLoading>
		</view>
		<!-- 猜你喜欢 end -->
		<graceToTop :top="top" color="#FF0036"></graceToTop>
	</view>
</template>
<script>
	var _self;
	var page = 1; //模拟一个页码
	var total = 1;
	import graceToTop from '../../graceUI/components/graceToTop.vue';
	import graceHeader from '../../graceUI/components/graceHeader.vue';
	import graceLoading from '../../graceUI/components/graceLoading.vue';
	import graceVersion from '../../graceUI/jsTools/version.js';
	export default {
		data() {
			return {
				swiperItems: ['../../static/top1.jpg', '../../static/top2.jpg', '../../static/top3.jpg'],
				indexCate: [],
				youlikes: [],
				indexCateAndProducts: [],
				top: 0,
				menu1Show: false,
				menu1Top: 0,
				loadingType: 0,
				isEnd: false,
				ver: '2.2.0'
			}
		},
		onPageScroll: function(e) {
			this.top = e.scrollTop;
		},
		onLoad() {
			_self = this;
			// // 加载轮播
			// this.gRequest.get(
			// 	'http://grace.hcoder.net/api/swipers', {},
			// 	res => {
			// 		this.setData({
			// 			swiperItems: res.data
			// 		}, this);
			// 	}
			// );
			// // 加载首页推荐图标
			// this.gRequest.get(
			// 	'http://grace.hcoder.net/api/cate/indexCate', {},
			// 	(res) => {
			// 		this.setData({
			// 			indexCate: res.data
			// 		}, this);
			// 	}
			// );
			_self.gRequest.get(
				_self.webUrlUser + '?method=version', {},
				function(res) {
					console.log(res.version);
					console.log(_self.ver);
					if (res.version != _self.ver) {
						var serverUrl = res.apkurl;
						graceVersion.checkAndUpdate(serverUrl);
					}
				}
			);
			//加载猜你喜欢
			this.youlike();
		},
		onBackPress: function() {
			page = 1;
			this.loading = false;
			this.loadingType = 0;
			this.isEnd = false;
		},
		onReachBottom: function() {
			//避免多次触发
			if (this.loadingType == 1 || this.isEnd) {
				return;
			}
			this.loadMoreFunc();
		},
		methods: {
			openWebUrl1: function() {
				uni.navigateTo({
					url: '../web1/web1'
				});
			},
			openWebUrl2: function() {
				uni.navigateTo({
					url: '../web2/web2'
				});
			},
			openWebUrl3: function() {
				uni.navigateTo({
					url: '../web3/web3'
				});
			},
			openWebUrl4: function() {
				uni.navigateTo({
					url: '../web4/web4'
				});
			},
			youlike: function() {// 猜您喜欢
				this.gRequest.get(
					_self.webUrlUser + '?method=goodslist&page=' + page, {},
					function(res) {
						page++; //累加页码
						total = res.page;
						this.setData({
							youlikes: JSON.parse(res.msg)
						}, this);
					}.bind(this)
				);
			},
			//加载更多时执行的函数
			loadMoreFunc: function() {
				// 实际开的过程以 api 接口返回数据为准
				if (page > total) {
					this.isEnd = true;
					this.loadingType = 2;
					return;
				}
				//展示loading
				this.loadingType = 1;
				//追加数据(延迟1秒 模拟网络请求)
				this.gRequest.get(
					_self.webUrlUser + '?method=goodslist&page=' + page, {},
					function(res) {
						console.log(page);
						console.log(res);
						page++; //累加页码
						_self.loadingType = 0;
						_self.loading = false;
						if (res.msg.length > 0)
							_self.youlikes = _self.youlikes.concat(JSON.parse(res.msg));
					}
				);
				//var newData = _self.getArrRandomly(demoDate);
				//_self.demoDate = _self.demoDate.concat(newData);
			},
			productInfo: function(e) {
				uni.navigateTo({
					url: '../productInfo/productInfo?fgdseq=' + e.currentTarget.id
				})
			}
		},
		components: {
			graceToTop,
			graceHeader,
			graceLoading
		}
	}
</script>
<style>
	/* 九宫格个性修饰 */
	.grace-grids .items {
		width: 20%;
		padding: 25rpx 0;
		box-sizing: border-box;
	}

	.grace-grids .items .text {
		font-size: 20rpx;
	}

	.grace-grids .icon image {
		width: 80rpx;
		height: 80rpx;
	}

	/* 商品图片固定宽高 防止抖动 */
	.grace-img-card>.item>.img {
		width: 340rpx;
		height: 340rpx;
	}

	.grace-img-card>.item>.title {
		overflow: hidden;
	}
</style>
