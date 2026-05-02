import { Link } from "react-router-dom";

export function BasketHeader() {
  return (
    <div className="w-full bg-white">
      <div className="w-[1200px] mx-auto py-5">
        <div className="flex flex-col">
          <div className="flex flex-row items-center">
            <Link to="/">
              <img
                src="src/public/logo-brand-no-bg.png"
                width="162px"
                height="50px"
              />
            </Link>
            <div className="h-10 mt-2 border-l-1 border-gray-900 ml-2 pr-3"></div>
            <div className="text-xl pt-4">Shoppping Cart</div>
          </div>
        </div>
      </div>
    </div>
  );
}
