// import Toast from 'bootstrap/js/dist/toast'

import { Toast } from 'bootstrap'

export function showToast(
  message: string,
  variant: 'primary' | 'success' | 'warning' | 'danger' = 'primary',
  opts?: Partial<Toast.Options>
): void {
  const container = document.querySelector('#toast-container')
  // eslint-disable-next-line curly
  if (!(container instanceof HTMLElement)) return

  const toast = document.createElement('div')
  toast.className = `toast text-bg-${variant}`
  toast.role = 'alert'
  toast.ariaLive = 'assertive'
  toast.ariaAtomic = 'true'

  toast.innerHTML = `
    <div class="d-flex">
      <div class="toast-body">${message}</div>
      <button type="button"
              class="btn-close btn-close-white me-2 m-auto"
              data-bs-dismiss="toast"
              aria-label="Close"></button>
    </div>
  `

  container.append(toast)

  const bsToast = new Toast(toast, opts)
  // ✅ тип Toast
  bsToast.show()
}
