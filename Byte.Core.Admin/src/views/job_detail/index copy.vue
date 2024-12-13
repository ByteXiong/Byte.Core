<script setup lang="tsx">
import { ref } from 'vue';
import { usePagination, useRequest } from 'alova/client';
import { useRoute } from 'vue-router';
import { type DataTableColumn, NButton, NDataTable, NSwitch, NTag } from 'naive-ui';
import dayjs from 'dayjs';
import { useAppStore } from '@/store/modules/app';
import { $t } from '@/locales';
import '@/api';
import { useAuth } from '@/hooks/business/auth';
import type { JobDetailDTO } from '@/api/globals';
import { TriggerActionEnum, TriggerStateEnum } from '@/api/apiEnums';
import EditForm from './modules/editForm.vue';
const { hasAuth } = useAuth();
// 获取当前页面路由参数
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
    Apis.JobDetail.get_api_jobdetail_getpage({
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
    Apis.JobDetail.delete_api_jobdetail_delete({
      data: ids,
      transform: res => {
        window.$message?.success('删除成功！');
        getData(page.value, pageSize.value);
        return res.data;
      }
    }),
  { force: true, immediate: false }
);

// 修改全局状态
const { send: setAllState } = useRequest(
  action =>
    Apis.JobDetail.post_api_jobdetail_setallstate({
      data: action,
      transform: res => {
        window.$message?.success('操作成功');
        getData(1, pageSize.value);
        return res.data;
      }
    }),
  { force: true, immediate: false }
);
// 修改作业状态
const { send: setJobState } = useRequest(
  action =>
    Apis.JobDetail.post_api_jobdetail_setjobstate({
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
// 修改触发器
const { send: setTriggerState } = useRequest(
  action =>
    Apis.JobDetail.post_api_jobdetail_settriggerstate({
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

const checkedRowKeys = ref<string[]>([]);

// 打开编辑/新增
const editFormRef = ref();
const openForm = (id?: string) => {
  editFormRef.value?.openForm(id);
};

// 触发器 处理
const columnTriggers = ref<Array<DataTableColumn & { checked?: boolean }>>([
  {
    key: 'groupName',
    title: $t('名称'),
    align: 'center',
    checked: true
  },
  {
    key: 'triggerType',
    title: $t('触发器类型'),
    align: 'center',
    checked: true
  },
  {
    key: 'assemblyName',
    title: $t('程序集'),
    align: 'center',
    checked: true
  },
  {
    key: 'props',
    title: $t('程序集'),
    align: 'center',
    checked: true
  },
  {
    key: 'startTime',
    title: $t('开始时间'),
    align: 'center',
    checked: true,
    render: row => {
      return row.startTime ? dayjs.unix(row.startTime).format('YYYY-MM-DD HH:mm:ss') : '';
    }
  },
  {
    key: 'endTime',
    title: $t('结束时间'),
    align: 'center',
    checked: true,
    render: row => {
      return row.endTime ? dayjs.unix(row.endTime).format('YYYY-MM-DD HH:mm:ss') : '';
    }
  },
  {
    key: 'LastRunTime',
    title: $t('最近运行时间'),
    align: 'center',
    checked: true,
    render: row => {
      return row.lastRunTime ? dayjs.unix(row.lastRunTime).format('YYYY-MM-DD HH:mm:ss') : '';
    }
  },
  {
    key: 'NextRunTime',
    title: $t('下次运行时间'),
    align: 'center',
    checked: true,
    render: row => {
      return row.nextRunTime ? dayjs.unix(row.nextRunTime).format('YYYY-MM-DD HH:mm:ss') : '';
    }
  },
  {
    key: 'numberOfRuns',
    title: $t('触发次数'),
    align: 'center',
    checked: true
  },

  {
    key: 'maxNumberOfRuns',
    title: $t('最大触发次数'),
    align: 'center',
    checked: true
  },

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
            setTriggerState({ id: row.id, action: value ? TriggerActionEnum.加入 : TriggerActionEnum.移除 })
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
    title: $t('状态'),
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
            <NButton size="small" onClick={() => setTriggerState({ id: row.id, action: TriggerActionEnum.启动 })}>
              启动
            </NButton>
            <NButton size="small" onClick={() => setTriggerState({ id: row.id, action: TriggerActionEnum.暂停 })}>
              暂停
            </NButton>
          </>
        ) : null}
      </div>
    )
  }
]);

const searchData = ref<Array<any>>([]);

const columns = ref<Array<DataTableColumn & { checked?: boolean }>>([
  {
    type: 'expand',
    checked: true,
    expandable: (row: JobDetailDTO) => (row?.triggers?.length ?? 0) > 0,
    renderExpand: (row: JobDetailDTO) => {
      return <NDataTable columns={columnTriggers.value} data={row.triggers || []} size="small" />;
    }
  },
  {
    key: 'groupName',
    title: $t('组名称'),
    align: 'center',
    checked: true
  },
  {
    key: 'jobType',
    title: $t('作业类型'),
    align: 'center',
    checked: true
  },
  {
    key: 'assemblyName',
    title: $t('程序集'),
    align: 'center',
    checked: true
  },
  {
    key: 'props',
    title: $t('额外参数'),
    align: 'center',
    checked: true,
    render: row => (
      <div class="flex-center gap-8px">
        <NButton size="small" onClick={() => setJobState({ id: row.id, action: TriggerActionEnum.启动 })}>
          启动
        </NButton>
        <NButton size="small" onClick={() => setJobState({ id: row.id, action: TriggerActionEnum.暂停 })}>
          暂停
        </NButton>
      </div>
    )
  }
]);
</script>

<template>
  <div class="flex-col-stretch gap-16px overflow-hidden lt-sm:overflow-auto">
    <TableHeaderSearch
      v-model:search-params="searchParams"
      :search-data="searchData"
      @reset="reload"
      @search="getData(1, pageSize)"
    />
    <NCard
      :title="$t(route.meta.i18nKey || route.meta.title || '')"
      :bordered="false"
      size="small"
      class="sm:flex-1-hidden card-wrapper"
    >
      <template #header-extra>
        任务调度是根据 UTC 时间启动, UTC ({{ dayjs(new Date(), { utc: true }) }})
        <TableHeaderOperation
          v-model:columns="columns"
          :disabled-delete="checkedRowKeys.length === 0"
          :loading="loading"
          @refresh="reload"
        >
          <NButton size="small" ghost type="primary" @click="setAllState(TriggerActionEnum.启动)">
            <template #icon>
              <icon-ic-round-plus class="text-icon" />
            </template>
            全部启动
          </NButton>
          <NButton size="small" ghost type="primary" @click="setAllState(TriggerActionEnum.暂停)">
            <template #icon>
              <icon-ic-round-plus class="text-icon" />
            </template>
            全部暂停
          </NButton>
          <NButton v-if="hasAuth('jobdetail/submit')" size="small" ghost type="primary" @click="openForm()">
            <template #icon>
              <icon-ic-round-plus class="text-icon" />
            </template>
            {{ $t('common.add') }}
          </NButton>
          <NPopconfirm v-if="hasAuth('jobdetail/delete')" @positive-click="handleDelete">
            <template #trigger>
              <NButton size="small" ghost type="error" :disabled="checkedRowKeys?.length === 0">
                <template #icon>
                  <icon-ic-round-delete class="text-icon" />
                </template>
                {{ $t('common.batchDelete') }}
              </NButton>
            </template>
            {{ $t('common.confirmDelete') }}
          </NPopconfirm>
        </TableHeaderOperation>
      </template>

      <NDataTable
        v-model:checked-row-keys="checkedRowKeys"
        :columns="columns.filter(item => item.checked)"
        :data="data"
        size="small"
        :flex-height="!appStore.isMobile"
        :scroll-x="1088"
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
    <EditForm ref="editFormRef" @refresh="getData(page, pageSize)"></EditForm>
  </div>
</template>

<style scoped></style>
