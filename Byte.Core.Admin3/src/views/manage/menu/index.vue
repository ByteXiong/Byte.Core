<script setup lang="tsx">
import { computed, ref } from 'vue';
import { useRequest } from 'alova/client';
import { useRoute } from 'vue-router';
import type * as Naive from 'naive-ui';
import { useAppStore } from '@/store/modules/app';
import { $t } from '@/locales';
import '@/api';
import type { MenuTreeDTO } from '@/api/globals';
import { ViewTypeEnum } from '@/api/apiEnums';
import customRender from '@/utils/customRender';
import { useAuth } from '@/hooks/business/auth';
import EditForm from './modules/editForm.vue';
const { hasAuth } = useAuth();
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
    Apis.Menu.delete_api_menu_delete({
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
const openForm = (id?: number | null) => {
  editFormRef.value?.openForm(id, 0);
};

// eslint-disable-next-line @typescript-eslint/no-unused-vars
const handleAddChildMenu = (row: MenuTreeDTO) => {
  editFormRef.value?.openForm(0, row.id);
};

// ====================开始处理动态生成=====================
// 共享函数
defineExpose({
  openForm,
  handleDelete,
  handleAddChildMenu
});

const searchData = ref<Array<any>>([]);
const columns = ref<Array<NaiveUI.TableColumnCheck>>([]);

const columnData = computed<Array<Naive.DataTableColumn>>(() => {
  return columns.value
    ?.filter(item => item.checked)
    .map(item => {
      const column = customRender(item.props);
      return {
        ...column,
        key: item.key,
        title: item.title
      } as Naive.DataTableColumn;
    });
});
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
          :disabled-delete="checkedRowKeys.length === 0"
          :loading="loading"
          @add="openForm()"
          @delete="handleDelete(checkedRowKeys)"
          @refresh="getData"
        >
          <template #prefix>
            <TableHeaderSetting
              v-model:columns="columns"
              v-model:search-data="searchData"
              :tableof="tableof"
              :view-type="ViewTypeEnum.主页"
            ></TableHeaderSetting>
          </template>

          <NButton v-if="hasAuth('menu/submit')" size="small" ghost type="primary" @click="openForm()">
            <template #icon>
              <icon-ic-round-plus class="text-icon" />
            </template>
            {{ $t('common.add') }}
          </NButton>
          <NPopconfirm v-if="hasAuth('menu/delete')" @positive-click="handleDelete">
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
