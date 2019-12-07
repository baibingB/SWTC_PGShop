<template>
	<view class="grace-body">
		<graceImmersedStatusbar bgColor="#F5F6F7"></graceImmersedStatusbar>
		<view class="grace-line-title" style="margin-top:80rpx;">
			<view class="line"></view>
			<view class="title grace-h4">购物车</view>
			<view class="line"></view>
		</view>
		<!-- 购物车为空 -->
		<view v-if="shoppingCard.items.length < 1" style="margin-top:150px;">
			<graceEmpty :iconSize="168" :iconType="4" iconColor="#E1E1E1"></graceEmpty>
			<view class="grace-text-center grace-margin-top">
				<text class="grace-text-small">您的购物车空空如也 ~ </text>
				<text class="grace-text-small grace-blue" style="margin-left:20rpx;">去购物</text>
			</view>
		</view>
		<!-- 购物车为空 -->
		<!-- 购物车主结构  -->
		<view class="grace-shoppingcard grace-margin-top" v-for="(item, index) in shoppingCard" :key="index">
			<view class="grace-title grace-flex-vcenter">
				<view><graceCheckBtn style="margin-right:12px;" @change="shopChange" :parameter="[index]" :data-shopindex="item.index" :checked="item.checked"></graceCheckBtn>{{item.shopName}}</view>
				<navigator class="grace-more">进店<text class="grace-icons icon-arrow-right"></text></navigator>
			</view>
			<view class="goods" v-for="(goods, indexItem) in item.items" :key="indexItem">
				<graceCheckBtn :checked="goods.checked" @change="itemChange" :parameter="[index+'-'+indexItem]" style="margin:12px 12px 0 0;"></graceCheckBtn>
				<image :src="goods.img" mode="widthFix"></image>
				<view class="body">
					<view class="goods-title">{{goods.goodsName}}</view>
					<view class="goods-price">
						PG {{goods.price}}
						<view class="goods-number">
							<graceNumberBox :disabled="true" :index="indexItem" :datas="index+''" :value="goods.count" :maxNum="1000" :minNum="1" v-on:change="numberChange"></graceNumberBox>
						</view>
					</view>
					<view class="goods-remove grace-icons icon-remove" @tap="removeGoods" :id="'removeIndex_' + index + '_' + indexItem+'_'+goods.goodsId"><text>删除</text></view>
				</view>
			</view>
		</view>
		<view style="height:120rpx;"></view>
		<!-- 底部  -->
		<view class="grace-footer grace-nowrap grace-flex-vcenter" v-if="shoppingCard.length >= 1">
			<view class="grace-shoppingcard-checkbtn grace-nowrap grace-flex-vcenter">
				<graceCheckBtn :checked="true" @change="itemChangeAll"></graceCheckBtn>
				<text class="grace-shoppingcard-check-text">{{selectText}}</text>
			</view>
			<view class="grace-shoppingcard-text">
				合计 : <text style="font-size:36upx; color:#F00;">PG {{totalprice}}</text>
			</view>
			<button type="warn" style="background:#E55D52; width:260rpx; flex-shrink:0;" @tap="checkout">立即结算</button>
		</view>
	</view>
</template>
<script>
var _self;
import graceNumberBox from "../../graceUI/components/graceNumberBox.vue";
import graceEmpty from "../../graceUI/components/graceEmpty.vue";
import graceCheckBtn from '../../graceUI/components/graceCheckBtn.vue';
import graceImmersedStatusbar from "../../graceUI/components/graceImmersedStatusbar.vue";
export default {
	data() {
		return {
			// 总价
			totalprice : '',
			// 选择文本
			selectText : '全不选',
			// 购物车数据 可以来自 api 请求或本地数据
			goodsShoplist:[],
			shoppingCard : [
				{
					"checked" : true,
					"shopName": "PG 官方旗舰店",
					"shopId": "S0001",
					"items": []
				}
			]
		}
	},
	onLoad : function(){
		_self = this;
	},
	onShow:function(){
		var fuserid = uni.getStorageSync('userid');
		if (fuserid.length <1) {
			uni.showToast({
				title:"请登录后使用购物车",
				icon:"none"
			});
			return;
		};
		_self.gRequest.get(
			_self.webUrlUser + '?method=goodsShoplist', {
				fuser: fuserid
			},
			function(res) {
				if(res.res =='0'){
					_self.shoppingCard =[];
					return;
				}
				let lgoodsShoplist = JSON.parse(res.goodsShoplist);
				_self.goodsShoplist = [];
				for (let i = 0; i < lgoodsShoplist.length; i++) {
					let item = {
						goodsId: lgoodsShoplist[i].FGDSEQ,
						goodsName: lgoodsShoplist[i].FGDNAME,
						price: lgoodsShoplist[i].FZKJJ,
						count: lgoodsShoplist[i].FSL,
						img: lgoodsShoplist[i].FURL,
						checked:true
					};
					_self.goodsShoplist.push(item);
				};
				_self.shoppingCard =[
					{
						"checked" : true,
						"shopName": "PG 官方店",
						"shopId": "S0001",
						"items":_self.goodsShoplist
					}
				];
				_self.countTotoal();// 初始化时候计算总价，如果使用api 获取购物车项目在 api 请求完成后执行此函数
			}
		);
	},
	methods:{
		//计算总计函数
		countTotoal:function(){
			var total = 0;
			for (var i = 0; i < this.shoppingCard.length; i++){
				for (var ii = 0; ii < this.shoppingCard[i].items.length; ii++){
					if(this.shoppingCard[i].items[ii].checked){
						total += Number(this.shoppingCard[i].items[ii].price) * Number(this.shoppingCard[i].items[ii].count);
					}
				}
			}
			this.totalprice = total;
		},
		numberChange:function(data){
			this.shoppingCard[data[2]].items[data[1]].count = data[0];
			var fuserid = uni.getStorageSync('userid');
			_self.gRequest.get(
				_self.webUrlUser + '?method=goodsShopChange', {
					fgdseq: _self.shoppingCard[data[2]].items[data[1]].goodsId,
					fuser: fuserid,
					fsl:data[0],
				},
				function(res) {
				}
			);
			//计算总价
			this.countTotoal();
		},
		removeGoods : function(e){
			var index = e.currentTarget.id.replace('removeIndex_', '');
			index = index.split('_');
			var index1 = Number(index[0]);
			var index2 = Number(index[1]);
			var fgdseq = index[2];
			var fuserid = uni.getStorageSync('userid');
			wx.showModal({
				title: '确认提醒',
				content: '您确定要移除此商品吗？',
				success:function(e){
					if (e.confirm) {
						console.log(fgdseq);
						_self.gRequest.get(
							_self.webUrlUser + '?method=goodsShopDel', {
								fgdseq: fgdseq,
								fuser: fuserid
							},
							function(res) {
								console.log(res);
								_self.shoppingCard[index1].items.splice(index2, 1);
								// 是否全部删除包含商铺
								if(_self.shoppingCard[index1].items.length < 1){
									_self.shoppingCard.splice(index1, 1);
								}
								//计算总价
								_self.countTotoal();
							}
						);
					}
				}
			})
		},
		checkout:function(){
			var goodsShoplistpost=[];
			for(let i = 0; i < _self.shoppingCard[0].items.length; i++){
				if(_self.shoppingCard[0].items[i].checked){
					let item = {
						goodsId: _self.shoppingCard[0].items[i].goodsId,
						goodsName: _self.shoppingCard[0].items[i].goodsName,
						price: _self.shoppingCard[0].items[i].price,
						count: _self.shoppingCard[0].items[i].count,
						img: _self.shoppingCard[0].items[i].img,
						checked:true
					};
					goodsShoplistpost.push(item);
				}
			};
			if(goodsShoplistpost.length<1){
				uni.showToast({
					title:"请选择需要结算的商品信息",
					icon:"none"
				});
				return;
			}
			var shoppingCardpost =[
				{
					"checked" : true,
					"shopName": "PG 官方店",
					"shopId": "S0001",
					"items":goodsShoplistpost
				}
			];
			uni.navigateTo({
				url:'../shopSuccess/shopSuccess?index='+JSON.stringify(shoppingCardpost)
			})
		},
		// 店铺选中按钮状态切换
		shopChange : function (e) {
			var index = Number(e[1]);
			this.shoppingCard[index].checked = e[0];
			for(let i = 0; i < this.shoppingCard[index].items.length; i++){
				this.shoppingCard[index].items[i].checked = e[0];
			}
			this.shoppingCard.splice(index, this.shoppingCard[index]);
			this.countTotoal();
		},
		// 商品选中
		itemChange : function (e) {
			var indexs = e[1].toString();
			var index = indexs.split('-');
			index[0] = Number(index[0]);
			index[1] = Number(index[1]);
			this.shoppingCard[index[0]].items[index[1]].checked = e[0];
			this.shoppingCard.splice(index[0], this.shoppingCard[index[0]]);
			var checkedNum = 0;
			for(let i = 0; i < this.shoppingCard[index[0]].items.length; i++){
				if(this.shoppingCard[index[0]].items[i].checked){
					checkedNum++;
				}
			}
			if(checkedNum < 1){
				this.shoppingCard[index[0]].checked = false;
			}else{
				this.shoppingCard[index[0]].checked = true;
			}
			this.shoppingCard = this.shoppingCard;
			this.countTotoal();
		},
		itemChangeAll : function (e) {
			this.selectText = e[0] ? '全不选' : '全选';
			for(var i = 0; i < this.shoppingCard.length; i++){
				this.shoppingCard[i].checked = e[0];
				for(var ii = 0; ii < this.shoppingCard[i].items.length; ii++){
					this.shoppingCard[i].items[ii].checked = e[0];
				}
			}
			this.countTotoal();
		}
	},
	components:{
		graceNumberBox,
		graceEmpty,
		graceCheckBtn,
		graceImmersedStatusbar
	}
}
</script>
<style>
page{background:#F5F6F7;}
.grace-shoppingcard-checkbtn{width:180rpx; margin-left:50rpx; flex-shrink:0;}
.grace-shoppingcard-check-text{font-size:28rpx; margin-left:12rpx; color:#666666;}
.grace-shoppingcard-text{width:700rpx; line-height:100rpx;}
.grace-line-title > .line{width:50rpx; flex:auto; height:1px; background:#D1D1D1; margin:0 50rpx;}
/* #ifdef H5 */
.grace-footer{bottom:50px;}
/* #endif */
</style>