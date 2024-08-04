<template>
  <div class="list_main_content">
    <el-card shadow="never" class="table-container">
      <div class="flex justify-between mb-2">
        <div class="flex justify-around w-1/8">
          <el-tooltip effect="light" :content="'新增'" placement="top">
            <!-- <el-button type="primary"  icon="Plus"  circle @click="openForm()" >  </el-button> -->
            <el-icon @click="openForm()"><Plus /></el-icon>
          </el-tooltip>
          <el-tooltip effect="light" content="批量删除" placement="top">
            <el-icon @click="handleDelete(selectIds)"><Delete /></el-icon>
          </el-tooltip>

          <!-- <el-button type="danger" :disabled="selectIds.length==0"  @click="handleDelete(selectIds)">批量删除</el-button> -->
        </div>
        <div class="flex justify-around w-1/8">
          <el-tooltip effect="light" content="刷新" placement="top">
            <el-icon @click="getData"><Refresh /></el-icon>
          </el-tooltip>
          <el-tooltip effect="light" content="导出" placement="top">
            <el-icon><Download /></el-icon>
          </el-tooltip>

          <!-- <el-tooltip effect="light" content="设置表头" placement="top">
            <el-icon><Setting /></el-icon>
          </el-tooltip> -->
        </div>
      </div>
      <el-table
        v-loading="loading"
        highlight-current-row
        row-key="id"
        :expand-row-keys="[0, 1]"
        :data="data"
        height="calc(100vh - 165px)"
        :border="true"
        default-expand-all
        :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
        @selection-change="handleSelectionChange"
        @row-dblclick="
          (row: Menu) => {
            openForm(row.id);
          }
        "
        ref="tableRef"
      >
        <el-table-column
          show-overflow-tooltip
          type="selection"
          width="55"
          class-name="onExcel"
        />
        <el-table-column show-overflow-tooltip label="菜单名称" min-width="200">
          <template #default="scope">
            <svg-icon :icon-class="scope.row.icon" />
            {{ scope.row.title }}
          </template>
        </el-table-column>

        <el-table-column show-overflow-tooltip label="类型" width="80">
          <template #default="scope">
            <el-tag :type="MenuTypeEl[scope.row.type]">
              {{ MenuTypeEnum[scope.row.type] }}</el-tag
            >
          </template>
        </el-table-column>

        <el-table-column
          show-overflow-tooltip
          label="路由路径"
          align="left"
          width="150"
          prop="path"
        />

        <el-table-column
          show-overflow-tooltip
          label="组件路径"
          align="left"
          width="250"
          prop="component"
        />

        <el-table-column
          show-overflow-tooltip
          label="权限标识"
          width="200"
          prop="perm"
        />

        <el-table-column show-overflow-tooltip label="状态" width="90">
          <template #default="scope">
            <el-switch
              v-model="scope.row.state"
              inline-prompt
              :loading="stateLoading"
              active-text="启用"
              inactive-text="禁用"
              @change="
                async (e: boolean) => {
                  setState(scope.row.id, e);
                }
              "
            />
          </template>
        </el-table-column>
        <el-table-column
          show-overflow-tooltip
          label="排序"
          width="80"
          prop="sort"
        />
        <el-table-column
          show-overflow-tooltip
          class-name="onExcel"
          label="操作"
          width="160"
          fixed="right"
        >
          <template #default="scope">
            <el-button
              v-hasPerm="['menu/update']"
              :loading="loading"
              type="primary"
              link
              size="small"
              @click.stop="openForm(scope.row.id)"
              ><i-ep-edit />编辑</el-button
            >
            <el-button
              v-hasPerm="['menu/delete']"
              :loading="loading"
              type="primary"
              link
              size="small"
              @click.stop="handleDelete([scope.row.id])"
              ><i-ep-delete />删除</el-button
            >
          </template>
        </el-table-column>

        <template #empty>
          <el-empty description="暂无数据" />
        </template>
      </el-table>

      <edit-form ref="editform" @refresh="getData" />
    </el-card>
  </div>
</template>
<script lang="ts" setup>
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import editForm from "./editForm.vue";
import { MenuTypeEnum } from "@/api/apiEnums";
import { MenuTypeEl } from "@/api/apiEls";
import "@/api";
import { Menu } from "@/api/globals";
defineOptions({
  name: "Menu",
  inheritAttrs: false,
});
/**
 * 获取数据
 */
const {
  send: getData,
  data,
  loading,
} = useRequest(
  () =>
    Apis.Menu.get_api_menu_gettree({
      transform: (res) => {
        return res.data;
      },
    }),
  {
    force: true,
    immediate: true,
  }
);
/**
 * 删除
 */
const { send: delIds } = useRequest(
  (ids: string[]) => Apis.Menu.delete_api_menu_delete({ data: ids }),
  { immediate: false }
);
/**
 * 设置状态
 */
const { send: setState, loading: stateLoading } = useRequest(
  (id: string, state: boolean) =>
    Apis.Menu.put_api_menu_setstate({
      params: { id, state },
      transform: (res) => {
        ElMessage.success("状态更新成功");
        return res.data;
      },
    }),
  {
    immediate: false,
  }
);

async function search() {
  await getData();
}

const selectIds = ref<string[]>([]);
//多选
async function handleSelectionChange(e: any) {
  selectIds.value = e.map((x: any) => x.id);
}

// 删除
async function handleDelete(ids: string[]) {
  if (ids.length == 0) {
    ElMessage.error("请勾选批量删除条目");
    return;
  }
  ElMessageBox.confirm("确定删除吗？", "删除", {
    confirmButtonText: "确认",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await delIds(ids);
    })
    .catch((error: any) => {
      console.log(error);
    });
}
//打开子页面
const editform = ref();
async function openForm(id?: string) {
  await editform.value.openForm(id);
}
</script>
