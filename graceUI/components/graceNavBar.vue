<template>
	<scroll-view 
	:class="['grace-nav-bar', isCenter ? 'grace-nav-center' : '']" :scroll-x="true" 
	:scroll-into-view="'tab-'+currentIndex">
		<view class="nav-item" :id="'tab-'+index" 
		:style="{width:size+'rpx', marginRight:margin+'rpx'}" v-for="(item, index) in items" 
		:key="index" @click="navchang" :data-index="index">
			<view class="nav-item-main">
				<view 
				:class="['nav-item-title', currentIndex == index ? 'nav-active' : '']" 
				:style="{color:currentIndex == index ? activeColor : color, textAlign : textAlign, lineHeight:lineHeight, fontSize:fontSize}"
				>{{item}}</view>
				<view class="nav-active-line" 
				:style="{background:activeLineBg, width:activeLineWidth, height:activeLineHeight}" 
				v-if="currentIndex == index"></view>
			</view>
		</view>
	</scroll-view>
</template>
<script>
export default {
	props:{
		isCenter : {
			type : Boolean,
			default : false
		},
		currentIndex : {
			type : Number,
			default : 0
		},
		size : {
			type : Number,
			default : 120
		},
		items : {
			type : Array,
			default : function () {
				return []
			}
		},
		activeLineBg : {
			type : String,
			default : "linear-gradient(to right, #66BFFF,#3388FF)"
		},
		activeColor:{
			type : String,
			default : "#333333"
		},
		activeLineHeight : {
			type : String,
			default : '6rpx'
		},
		activeLineWidth : {
			type : String,
			default : "36rpx"
		},
		color : {
			type : String,
			default : "#333333"
		},
		margin : {
			type : Number,
			default : 0
		},
		textAlign : {
			type : String,
			default : 'left'
		},
		lineHeight : {
			type : String,
			default : '50rpx'
		},
		fontSize : {
			type : String,
			default : '28rpx'
		}
	},
	methods:{
		navchang : function (e){
			this.$emit('change', Number(e.currentTarget.dataset.index))
		}
	}
}
</script>
<style>
.grace-nav-bar{width:100%; display:flex; white-space:nowrap; padding-bottom:10rpx;}
.nav-item{width:100rpx; display:inline-flex;}
.nav-item-main{width:100%;}
.nav-item-title{font-size:28rpx; line-height:50rpx; width:100%; color:#333333;}
.nav-active{font-size:30rpx; font-weight:bold;}
.nav-active-line{width:36rpx; height:6rpx; margin-top:5rpx;}
.grace-nav-center{justify-content:center; text-align:center;}
</style>