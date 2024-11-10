<script setup lang="tsx">
import { computed, h, ref } from 'vue';
import { useRequest } from 'alova/client';
import { useRoute } from 'vue-router';
import * as Naive from 'naive-ui';
import { useAppStore } from '@/store/modules/app';
import { $t } from '@/locales';
import '@/api';
import EditForm from './modules/editForm.vue';

// 获取当前页面路由参数
const route = useRoute();
const tableof = ref('MenuTreeDTO');
const searchParams = ref<NaiveUI.SearchParams>({});
/** 获取数据 */
const {
  send: getData,
  data,
  loading
} = useRequest(
  () =>
    Apis.Menu.get_api_menu_gettree({
      transform: res => {
        return res.data;
      }
    }),
  {
    force: true,
    immediate: true
  }
);
// 删除
const { send: handleDelete } = useRequest(
  ids =>
    Apis.User.delete_api_user_delete({
      data: ids,
      transform: res => {
        window.$message?.success('删除成功！');
        getData();
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

// ====================开始处理动态生成=====================
const searchData = ref<Array<any>>([]);
const columns = ref<Array<NaiveUI.TableColumnCheck>>([]);

// eslint-disable-next-line @typescript-eslint/no-unused-vars, @typescript-eslint/no-shadow
const customRender = (str?: string, h?: unknown, Naive?: any) => {
  // eslint-disable-next-line no-eval
  return eval(`(${str || '{}'})`);
};
const columnData = computed<Array<Naive.DataTableColumn>>(() => {
  return columns.value
    ?.filter(item => item.checked)
    .map(item => {
      const column = customRender(item.props, h, Naive);
      return {
        ...column,
        key: item.key,
        title: item.title
      } as Naive.DataTableColumn;
    });
});
// const column = customRender(item.props, h, Naive);
//           console.error(column);
//           // console.log(JSON.parse(item.props || '{}'));
//           return {
//             ...column,
</script>

<template>
  <div class="flex-col-stretch gap-16px overflow-hidden lt-sm:overflow-auto">
    <TableHeaderSearch
      v-model:search-params="searchParams"
      :search-data="searchData"
      @reset="getData"
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
          @add="openForm"
          @delete="handleDelete(checkedRowKeys)"
          @refresh="getData"
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
        :columns="columnData"
        :data="data"
        size="small"
        :flex-height="!appStore.isMobile"
        :scroll-x="1088"
        :loading="loading"
        :row-key="row => row.id"
        remote
        class="sm:h-full"
      />
    </NCard>
    <EditForm ref="editFormRef"></EditForm>
  </div>
</template>

<style scoped></style>
