<template>
  <div class="list_main_content">
    <!-- <div class="search-container">
      <el-form ref="queryFormRef" :model="param" :inline="true">
        <el-form-item label="关键字" prop="name">
          <el-input
            v-model="param.keyWord"
            placeholder="字典类型名称/编码"
            clearable
            @keyup.enter="search"
          />
        </el-form-item>
        <el-form-item>
          <el-button :loading="loading" type="primary" @click="search"
            ><i-ep-search />搜索</el-button
          >
          <el-button :loading="loading" @click="refresh"><i-ep-refresh />重置</el-button>
        </el-form-item>
      </el-form>
    </div> -->
    <el-card shadow="never" class="table-container">
      <template #header>
        <div>
          <el-button
            :loading="loading"
            v-hasPerm="['menu/add']"
            type="success"
            @click="openForm()"
            ><i-ep-plus />新增</el-button
          >
          <el-button
            v-hasPerm="['menu/update']"
            :loading="loading"
            type="primary"
            :disabled="selectIds.length === 0"
            @click="openForm(selectIds[selectIds.length - 1])"
            ><i-ep-edit />编辑</el-button
          >
          <el-button
            v-hasPerm="['menu/delete']"
            :loading="loading"
            type="danger"
            :disabled="selectIds.length === 0"
            @click="handleDelete(selectIds)"
            ><i-ep-delete />删除</el-button
          >
        </div>
      </template>
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

        <el-table-column
          show-overflow-tooltip
          label="类型"
          align="center"
          width="80"
        >
          <template #default="scope">
            <el-tag :type="MenuTypeEl[scope.row.type]">
              {{ MenuTypeEnum[scope.row.type] }}</el-tag
            >

            <!-- <el-tag v-if="scope.row.type === MenuType." type="warning"
              >目录</el-tag
            >
            <el-tag v-if="scope.row.type === MenuType.MENU" type="success"
              >菜单</el-tag
            >
            <el-tag v-if="scope.row.type === MenuType.BUTTON" type="danger"
              >按钮</el-tag
            >
            <el-tag v-if="scope.row.type === MenuType.EXTLINK" type="info"
              >外链</el-tag
            > -->
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
          align="center"
          width="200"
          prop="perm"
        />

        <el-table-column
          show-overflow-tooltip
          label="状态"
          align="center"
          width="90"
        >
          <template #default="scope">
            <el-switch
              v-model="scope.row.state"
              inline-prompt
              active-text="启用"
              inactive-text="禁用"
              @change="
                async (e: boolean) => {
                  setState(scope.row.id, e);
                  // getData();
                }
              "
            />
          </template>
        </el-table-column>

        <el-table-column
          show-overflow-tooltip
          label="排序"
          align="center"
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
const { send: setState } = useRequest(
  (id: string, state: boolean) =>
    Apis.Menu.put_api_menu_setstate({ params: { id, state } }),
  {
    immediate: false,
  }
);

async function search() {
  await getData();
}

const MenuTypeEl: Record<number, "success" | "warning" | "info" | "danger"> = {
  [MenuTypeEnum.菜单]: "warning",
  [MenuTypeEnum.目录]: "success",
  [MenuTypeEnum.按钮]: "danger",
  [MenuTypeEnum.外链]: "info",
};

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
<style lnag="scss" scoped>
:deep(.el-switch.is-checked .el-switch__core) {
  background: #13ce66;
}
:deep(.el-switch__core) {
  background: #ff4949;
  padding: 10px 5px;
  border-radius: 10px;
}
</style>
