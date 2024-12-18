<script setup lang="tsx">
import { ref, watch } from 'vue';
import { usePagination, useRequest } from 'alova/client';
import { useRoute } from 'vue-router';
import { type DataTableColumn, NButton, NDataTable } from 'naive-ui';
import { useAppStore } from '@/store/modules/app';
import { $t } from '@/locales';
import '@/api';
import { useAuth } from '@/hooks/business/auth';
import { TriggerActionEnum } from '@/api/apiEnums';
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

const appStore = useAppStore();

// const { bool: visible, setTrue: openModal } = useBoolean();

const checkedRowKeys = ref<string[]>([]);

// 打开编辑/新增
const editFormRef = ref();
const openForm = (id?: string) => {
  editFormRef.value?.openForm(id);
};

const columns = ref<Array<DataTableColumn & { checked?: boolean }>>([
  {
    type: 'selection',
    multiple: false,
    width: 20
  },
  {
    key: 'groupName',
    title: $t('组名称'),
    align: 'center',
    width: 80,
    checked: true
  },
  // {
  //   key: 'jobType',
  //   title: $t('作业类型'),
  //   align: 'center',
  //   checked: true,
  //   width: 100
  // },
  {
    key: 'assemblyName',
    title: $t('程序集'),
    align: 'center',
    width: 180,
    ellipsis: {
      tooltip: true
    },
    checked: true
  }
  // {
  //   key: 'props',
  //   title: $t('操作'),
  //   align: 'center',
  //   checked: true,
  //   render: row => (
  //     <div class="flex-center gap-8px">
  //       <NButton size="small" onClick={() => setJobState({ id: row.id, action: TriggerActionEnum.启动 })}>
  //         启动
  //       </NButton>
  //       <NButton size="small" onClick={() => setJobState({ id: row.id, action: TriggerActionEnum.暂停 })}>
  //         暂停
  //       </NButton>
  //     </div>
  //   )
  // }
]);
// 选中行
interface Emits {
  (e: 'handleCheck', row: any): any;
}
const emit = defineEmits<Emits>();
watch(
  () => checkedRowKeys.value,
  () => {
    emit('handleCheck', checkedRowKeys.value);
  }
);
</script>

<template>
  <NCard :title="$t(route.meta.i18nKey || route.meta.title || '')" :bordered="false" size="small" class="h-full">
    <template #header-extra>
      <div class="flex-center gap-8px">
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
      </div>
    </template>

    <NDataTable
      v-model:checked-row-keys="checkedRowKeys"
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
