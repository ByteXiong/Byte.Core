<script setup lang="tsx">
import { ref } from 'vue';
import { usePagination, useRequest } from 'alova/client';
import { useRoute, useRouter } from 'vue-router';
import { type DataTableColumn, NButton, NDataTable, NSwitch, NTag } from 'naive-ui';
import dayjs from 'dayjs';
import { useAppStore } from '@/store/modules/app';
import { $t } from '@/locales';
import '@/api';
import { useAuth } from '@/hooks/business/auth';
import { TriggerActionEnum, TriggerStateEnum, TriggerTypeEnum } from '@/api/apiEnums';

import type { JobTriggerDTO } from '@/api/globals';
const { hasAuth } = useAuth();
// 获取当前页面路由参数`
const route = useRoute();
const searchParams = ref<NaiveUI.SearchParams>({});
const keyWord = ref('');
const sortList = ref<Record<string, string>>({ id: 'asc' });
/** 获取数据 */
const {
  data,
  page,
  pageSize,
  total: itemCount,
  loading,
  send: getData,
  reload
} = usePagination(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  (upPageIndex, upPageSize) =>
    Apis.JobTrigger.get_api_jobtrigger_getpage({
      params: {
        PageIndex: upPageIndex,
        pageSize: upPageSize,
        sortList: sortList.value,
        search: searchParams.value
      }
    }),
  {
    watchingStates: [keyWord, sortList],
    // 请求前的初始数据（接口返回的数据格式）
    // initialData: {
    //   pagerInfo: {
    //     pageIndex: 1,
    //     pageSize: 10,
    //     totalRowCount: 0,
    //   },
    //   data: [],
    // },
    force: true,
    initialPage: 1, // 初始页码，默认为1
    initialPageSize: 20, // 初始每页数据条数，默认为10
    preloadPreviousPage: false, // 是否预加载下一页
    preloadNextPage: false, // 是否预加载上一页
    total: res => res.data?.pagerInfo?.totalRowCount,
    data: res => res.data?.data
  }
);
// 删除
const { send: handleDelete } = useRequest(
  ids =>
    Apis.JobTrigger.delete_api_jobtrigger_delete({
      data: ids,
      transform: res => {
        window.$message?.success('删除成功！');
        getData(page.value, pageSize.value);
        return res.data;
      }
    }),
  { force: true, immediate: false }
);

// 修改触发器
const { send: setState } = useRequest(
  action =>
    Apis.JobTrigger.post_api_jobtrigger_setstate({
      params: { id: action.id },
      data: action.action,
      transform: res => {
        window.$message?.success('操作成功');
        getData(page.value, pageSize.value);
        return res.data;
      }
    }),
  { force: true, immediate: false }
);

const appStore = useAppStore();

// const { bool: visible, setTrue: openModal } = useBoolean();

// const checkedRowKeys = ref<string[]>([]);

// 打开编辑/新增
// const editFormRef = ref();
// const openForm = (id?: string) => {
//   editFormRef.value?.openForm(id);
// };

const columns = ref<Array<DataTableColumn & { checked?: boolean }>>([
  {
    key: 'groupName',
    title: $t('名称'),
    align: 'center',
    checked: true
  },
  {
    key: 'triggerType',
    title: $t('类型'),
    align: 'center',
    checked: true,
    render: (row: JobTriggerDTO) => {
      return <NTag> {TriggerTypeEnum[row.triggerType as number]}</NTag>;
    }
  },
  {
    key: 'props',
    title: $t('参数'),
    align: 'center',

    ellipsis: {
      tooltip: true
    },
    checked: true
  },
  {
    key: 'startTime',
    title: $t('开始时间'),
    align: 'center',
    width: 120,
    checked: true,
    render: (row: JobTriggerDTO) => {
      return row.startTime ? dayjs.unix(row.startTime).format('MM-DD HH:mm:ss') : '';
    }
  },
  {
    key: 'endTime',
    title: $t('结束时间'),
    align: 'center',
    checked: true,
    width: 120,
    render: (row: JobTriggerDTO) => {
      return row.endTime ? dayjs.unix(row.endTime).format('MM-DD HH:mm:ss') : '';
    }
  },
  {
    key: 'LastRunTime',
    title: $t('最近运行时间'),
    align: 'center',
    checked: true,
    width: 120,
    render: (row: JobTriggerDTO) => {
      return row.lastRunTime ? dayjs.unix(row.lastRunTime).format('MM-DD HH:mm:ss') : '';
    }
  },
  {
    key: 'NextRunTime',
    title: $t('下次运行时间'),
    align: 'center',
    width: 120,
    checked: true,
    render: (row: JobTriggerDTO) => {
      return row.nextRunTime ? dayjs.unix(row.nextRunTime).format('MM-DD HH:mm:ss') : '';
    }
  },
  {
    key: 'numberOfRuns',
    title: $t('触发次数'),
    align: 'center',
    checked: true
  },

  // {
  //   key: 'maxNumberOfRuns',
  //   title: $t('最大触发次数'),
  //   align: 'center',
  //   checked: true
  // },

  // {
  //   key: 'numberOfErrors',
  //   title: $t('出错次数'),
  //   align: 'center',
  //   checked: true
  // },

  // {
  //   key: 'maxNumberOfErrors',
  //   title: $t('最大出错次数'),
  //   align: 'center',
  //   checked: true
  // },

  // {
  //   key: 'numRetries',
  //   title: $t('重试次数'),
  //   align: 'center',
  //   checked: true
  // },
  // {
  //   key: 'retryTimeout',
  //   title: $t('重试间隔时间'),
  //   align: 'center',
  //   checked: true
  // },
  // {
  //   key: 'startNow',
  //   title: $t('是否立即启动'),
  //   align: 'center',
  //   checked: true
  // },
  // {
  //   key: 'runOnStart',
  //   title: $t('是否启动时执行一次'),
  //   align: 'center',
  //   checked: true
  // },
  {
    key: 'status',
    title: $t('状态'),
    align: 'center',
    checked: true,
    render: row => {
      return (
        <NSwitch
          v-model:value={row.status}
          onUpdate:value={value =>
            setState({ id: row.id, action: value ? TriggerActionEnum.加入 : TriggerActionEnum.移除 })
          }
        >
          {{
            checked: () => '移除',
            unchecked: () => '加入'
          }}
        </NSwitch>
      );
    }
  },

  {
    key: 'state',
    title: $t('运行状态'),
    align: 'center',
    checked: true,
    render: row => {
      return <NTag> {TriggerStateEnum[row.state as number]}</NTag>;
    }
  },
  {
    key: 'type',
    title: $t('操作'),
    align: 'center',
    checked: true,
    fixed: 'right',
    render: row => (
      <div class="flex-center gap-8px">
        {row.status ? (
          <>
            <NButton size="small" onClick={() => setState({ id: row.id, action: TriggerActionEnum.启动 })}>
              启动
            </NButton>
            <NButton size="small" onClick={() => setState({ id: row.id, action: TriggerActionEnum.暂停 })}>
              暂停
            </NButton>
          </>
        ) : null}
      </div>
    )
  }
]);
// 作业看板
const router = useRouter();
const openRecord = async (action: TriggerActionEnum) => {
  router.push({
    path: `/manage/JobTriggerRecord`,
    query: {
      configId: 'Byte.Core'
    }
  });
};
</script>

<template>
  <NCard :title="$t('触发器')" :bordered="false" size="small" class="sm:h-full">
    <template #header-extra>
      <div class="flex-center gap-8px">
        <NButton size="small" ghost type="primary" @click="openRecord(TriggerActionEnum.启动)">
          <template #icon>
            <icon-ic-round-plus class="text-icon" />
          </template>
          作业看板
        </NButton>
      </div>
    </template>

    <NDataTable
      :columns="columns"
      :data="data"
      size="small"
      :flex-height="!appStore.isMobile"
      :loading="loading"
      :row-key="row => row.id"
      remote
      class="sm:h-full"
      :pagination="{
        page,
        pageSize,
        showSizePicker: true,
        itemCount,
        pageSizes: [20, 50, 100, 500, 1000],
        onUpdatePage(value) {
          page = value;
        },
        onUpdatePageSize(value) {
          page = 1;
          pageSize = value;
        }
      }"
    />
  </NCard>
</template>

<style scoped></style>
