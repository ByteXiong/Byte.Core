<!-- eslint-disable @typescript-eslint/no-unused-vars -->
<script setup lang="ts">
import { computed, ref } from 'vue';
import { useRequest } from 'alova/client';

import dayjs from 'dayjs';
import type { DataTableColumn } from 'naive-ui';
import { useAppStore } from '@/store/modules/app';
import HeaderBanner from './modules/header-banner.vue';
import CardData from './modules/card-data.vue';
import LineChart from './modules/line-chart.vue';
import PieChart from './modules/pie-chart.vue';
import ProjectNews from './modules/project-news.vue';
import CreativityBanner from './modules/creativity-banner.vue';
import '@/api';

const appStore = useAppStore();
const timestamp = ref(dayjs().valueOf());
/** 获取日榜数据 */
const {
  data: dayData,
  loading: dayLoading,
  send: getDayData
} = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () =>
    Apis.Rank.get_api_rank_day({
      params: {
        timestamp: timestamp.value
      },
      transform: res => {
        return res.data;
      }
    }),
  {
    force: true,
    immediate: true
  }
);
const columns = ref<Array<DataTableColumn>>([
  {
    title: '用户id',
    key: 'userId',
    align: 'center'
  },
  {
    title: '排名',
    key: 'sort',
    align: 'center'
  },
  {
    title: '用户昵称',
    key: 'nickName',
    align: 'center'
  },
  {
    title: '连赢次数',
    key: 'win',
    align: 'center'
  },
  {
    title: '消耗宝石',
    key: 'gems',
    align: 'center'
  },
  {
    title: '等级',
    key: 'vip',
    align: 'center'
  }
]);

/** 获取周榜数据 */
const {
  data: weekData,
  loading: weekLoading,
  send: getWeekData
} = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () =>
    Apis.Rank.get_api_rank_week({
      params: {
        timestamp: timestamp.value
      },
      transform: res => {
        return res.data;
      }
    }),
  {
    force: true,
    immediate: true
  }
);
const gap = computed(() => (appStore.isMobile ? 0 : 16));
</script>

<template>
  <NSpace vertical :size="16">
    <HeaderBanner />
    <!-- <CardData /> -->
    <!--
 <NGrid :x-gap="gap" :y-gap="16" responsive="screen" item-responsive>
      <NGi span="24 s:24 m:14">
        <NCard :bordered="false" class="card-wrapper">
          <LineChart />
        </NCard>
      </NGi>
      <NGi span="24 s:24 m:10">
        <NCard :bordered="false" class="card-wrapper">
          <PieChart />
        </NCard>
      </NGi>
    </NGrid>
-->
    <NGrid :x-gap="gap" :y-gap="16" responsive="screen" item-responsive>
      <NGi span="24 s:24 m:12">
        <NCard title="日排行榜">
          <NDataTable :columns="columns" :data="dayData" :single-line="false" />
        </NCard>
      </NGi>
      <NGi span="24 s:24 m:12">
        <NCard title="周排行榜">
          <NDataTable :columns="columns" :data="weekData" :single-line="false" />
        </NCard>
      </NGi>
    </NGrid>
  </NSpace>
</template>

<style scoped></style>
