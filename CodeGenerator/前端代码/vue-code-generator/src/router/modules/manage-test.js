//  测试管理

import Layout from '@/layout'

const componentsRouter = {
  path: '/tests',
  component: Layout,
  redirect: 'noRedirect',
  name: 'Test',
  meta: {
    title: 'test',
    icon: 'component'
  },
  children: [
    {
      path: 'test',
      component: () => import('@/views/manage-test/host-menu-test'),
      name: 'Test',
      meta: { title: 'test' }
    }
  ]
}

export default componentsRouter
