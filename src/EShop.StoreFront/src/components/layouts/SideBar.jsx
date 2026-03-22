export function SideBar() {
  return (
    <div className="w-[300px] h-screen bg-white fixed top-0 left-0">
      <div className="p-4">
        <div className="mb-4">
          <div className="flex gap-2">
            <div>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width={24}
                height={24}
                viewBox="0 0 24 24"
              >
                <path
                  fill="#000"
                  d="M18 6h-2c0-2.21-1.79-4-4-4S8 3.79 8 6H6c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V8c0-1.1-.9-2-2-2m-8 4c0 .55-.45 1-1 1s-1-.45-1-1V8h2zm2-6c1.1 0 2 .9 2 2h-4c0-1.1.9-2 2-2m4 6c0 .55-.45 1-1 1s-1-.45-1-1V8h2z"
                ></path>
              </svg>
            </div>
            <h2 className="text-lg">Product Management</h2>
          </div>
        </div>
        <nav className="flex flex-col text-sm gap-2 ml-8 mt-2">
          <a
            href="/portal/product/list"
            className="text-gray-700 hover:text-gray-900"
          >
            All Products
          </a>
          <a
            href="/portal/product/new"
            className="text-gray-700 hover:text-gray-900"
          >
            Add Product
          </a>
        </nav>
      </div>
    </div>
  );
}
