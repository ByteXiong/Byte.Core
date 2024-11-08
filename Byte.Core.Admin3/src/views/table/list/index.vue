<script setup lang="tsx">
import { ref } from 'vue';
import { usePagination } from 'alova/client';
import { useRoute } from 'vue-router';
import { useAppStore } from '@/store/modules/app';
import { $t } from '@/locales';
import '@/api';

// 获取当前页面路由参数
const route = useRoute();
const tableof = ref(route.path.split('/').pop());
const searchParams = ref<NaiveUI.SearchParams>({});
const keyWord = ref('');
const sortList = ref<Record<string, string>>({ id: 'asc' });
/** 获取数据 */
const {
  data,
  page,
  pageSize,
  total,
  loading,
  send: getData,
  reload
} = usePagination(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  (upPageIndex, upPageSize) =>
    Apis.DataTable.get_api_datatable_page({
      params: {
        Table: tableof.value,
        pageIndex: upPageIndex,
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
    initialPageSize: 10, // 初始每页数据条数，默认为10
    preloadPreviousPage: false, // 是否预加载下一页
    preloadNextPage: false, // 是否预加载上一页
    total: res => res.data?.pagerInfo?.totalRowCount,
    data: res => res.data?.data
  }
);

const appStore = useAppStore();

// const { bool: visible, setTrue: openModal } = useBoolean();

const wrapperRef = ref<HTMLElement | null>(null);
const searchData = ref<Array<any>>([]);

const columns = ref<Array<NaiveUI.TableColumnCheck>>([]);

const checkedRowKeys = ref<string[]>([]);
// const { checkedRowKeys, onBatchDeleted, onDeleted } = useTableOperate(data, getData);

// const operateType = ref<OperateType>('add');

function handleAdd() {
  // operateType.value = 'add';
  // openModal();
}

async function handleBatchDelete() {}

function init() {
  getData(1, 10);
  // getAllPages();
}

// init
init();
</script>

<template>
  <div ref="wrapperRef" class="flex-col-stretch gap-16px overflow-hidden lt-sm:overflow-auto">
    <TableHeaderSearch
      v-model:search-params="searchParams"
      :search-data="searchData"
      @reset="reload"
      @search="getData"
    />

    <NCard
      :title="$t(route.meta.i18nKey || route.meta.title || '')"
      :bordered="false"
      size="small"
      class="sm:flex-1-hidden card-wrapper"
    >
      <template #header-extra>
        <TableHeaderOperation
          v-model:columns="columns"
          tableof="TableColumnDTO"
          :disabled-delete="checkedRowKeys.length === 0"
          :loading="loading"
          @add="handleAdd"
          @delete="handleBatchDelete"
          @refresh="reload"
        >
          <template #prefix>
            <TableHeaderSetting
              v-model:columns="columns"
              v-model:search-data="searchData"
              :tableof="tableof"
            ></TableHeaderSetting>
          </template>
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
          page: page,
          pageSize,
          itemCount: total,
          showSizePicker: true,
          pageSizes: [10, 20, 50, 100],
          onChange: (p: number) => {

          },
          onUpdatePageSize: (pageSize: number) => {

          }
        }"
      />
    </NCard>
  </div>
</template>

<style scoped></style>
