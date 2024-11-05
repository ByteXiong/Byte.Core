<script setup lang="ts" generic="T extends Record<string, unknown>, K = never">
import { VueDraggable } from 'vue-draggable-plus';
import { useForm, useRequest } from 'alova/client';
import { ref, watch } from 'vue';
import type { DataTableColumn, DataTableColumns } from 'naive-ui/es/data-table';
import { $t } from '@/locales';
import type { TableColumn, TableModel } from '@/api/globals';
defineOptions({
  name: 'TableColumnSetting'
});

const columns = defineModel<DataTableColumns>('columns', {
  default: () => []
});
interface Props {
  tableof?: string;
}

const props = defineProps<Props>();
const isSwitch = ref<boolean>(false);
const visible = ref<boolean>(false);
// 获取数据
const { data: dataColumns } = useRequest(
  () =>
    Apis.TableColumn.get_api_tablecolumn_getcolumns({
      params: {
        Table: props.tableof,
        Router: '/demo'
      },
      transform: res => {
        return (
          res.data.columns?.map(col => {
            return col as TableColumn & {
              isDrawer: boolean;
            };
          }) || []
        );
      }
    }),
  {
    debounce: 500,
    immediate: true,
    async middleware(_, next) {
      if (props.tableof) {
        await next();
      } else {
        console.error('tableColumn组件 请配置tableof 返回模型DTO');
      }
    }
  }
);
const { send: submit } = useForm(
  () =>
    Apis.TableColumn.post_api_tablecolumn_setcolumns({
      data: {
        table: props.tableof,
        router: '/demo',
        Columns: dataColumns.value
      },
      transform: () => {
        window.$message?.success('保存成功！');
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: false,
    initialForm: {} as TableModel
    // async middleware(context, next) {
    //   console.log(context);

    //   await next();
    // }
  }
);

watch(
  () => dataColumns.value,
  () => {
    columns.value =
      dataColumns.value
        ?.filter(col => col.isShow)
        ?.map(col => {
          // const porp = eval(col.prop);
          return {
            title: col.title,
            key: col.key
          } as DataTableColumn;
        }) || [];
  },
  { deep: true }
);
</script>

<template>
  <NPopover placement="bottom-end" trigger="click" :show="visible">
    <template #trigger>
      <NButton size="small" @click="visible = true">
        <template #icon>
          <icon-ant-design-setting-outlined class="text-icon" />
        </template>
        {{ $t('common.columnSetting') }}
      </NButton>
    </template>
    <template #header>
      <div class="flex justify-between">
        <NSwitch v-model:value="isSwitch">
          <template #checked>高级</template>
          <template #unchecked>基础</template>
        </NSwitch>
        <icon-mdi-close class="mr-8px h-full cursor-move text-icon" @click="visible = false" />
      </div>
    </template>
    <VueDraggable v-model="dataColumns" :animation="150" filter=".none_draggable">
      <div
        v-for="(item, index) in dataColumns"
        :key="index"
        class="h-36px flex-y-center rd-4px hover:(bg-primary bg-opacity-20)"
      >
        <icon-mdi-drag class="mr-8px h-full cursor-move text-icon" />
        <NCheckbox v-model:checked="item.isShow" class="none_draggable flex-1">
          {{ item.title }}
        </NCheckbox>
        <icon-ant-design-setting-outlined v-if="isSwitch" class="text-icon" @click="item.isDrawer = true" />
        <TableColumnFrom v-model:show="item.isDrawer" :column="item"></TableColumnFrom>
      </div>
    </VueDraggable>

    <div v-if="isSwitch" class="flex justify-end">
      <NButton size="small" type="primary" @click="submit">
        <template #icon>
          <icon-ant-design-setting-outlined class="text-icon" />
        </template>
        保存
      </NButton>
    </div>
  </NPopover>
</template>

<style scoped></style>
