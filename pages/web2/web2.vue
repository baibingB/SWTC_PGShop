<template>
	<view>
		<web-view :webview-styles="webviewStyles" :src="url"></web-view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				url: '',
				webviewStyles: {
					progress: {
						color: '#000'
					}
				}
			}
		},
		onLoad(e) {
			this.url = 'https://wechatsq-1300381311.cos-website.ap-chengdu.myqcloud.com/register?code=PIUX65#/register';
		}
	}
</script>
