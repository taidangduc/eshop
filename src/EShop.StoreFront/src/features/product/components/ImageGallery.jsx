import { useProductImage } from "../context";
import clsx from "clsx";
import arrowLeft from "@public/arrow_left.svg";
import arrowRight from "@public/arrow_right.svg";
import { Image } from "@components/ui";
import { useState } from "react";

export const ImageGallery = ({ images = [], limit }) => {
  //context
  const { apply } = useProductImage();

  const [galleryIndex, setGalleryIndex] = useState(0);
  const [currentIndex, setCurrentIndex] = useState(0);

  const imageList = images.slice(galleryIndex, galleryIndex + limit);
  const canShowButton = images && images.length > limit;

  const nextIndexInGallery = () => {
    if (galleryIndex + limit < images.length) {
      setGalleryIndex(galleryIndex + 1);
    }
  };

  const prevIndexInGallery = () => {
    if (galleryIndex > 0) {
      setGalleryIndex(galleryIndex - 1);
    }
  };

  const selectImageByIndex = (index, image) => {
    setCurrentIndex(index);
    apply(image);
  };

  return (
    <div className="relative grid grid-cols-5 mt-3 gap-auto w-full">
      {imageList.map((img, i) => {
        const __index = galleryIndex + i;
        const __active = __index === currentIndex;

        return (
          <div
            key={i}
            className="w-[92px] h-[92px] p-[5px] cursor-pointer aspect-square"
            onMouseEnter={() => selectImageByIndex(__index, img)}
            onMouseLeave={() => {}}
            onClick={() => selectImageByIndex(__index, img)}
          >
            <Image
              src={img.imageUrl}
              alt={`Thumbnail ${i}`}
              className={clsx(
                "w-full h-full object-contain",
                __active && "outline outline-2 outline-gray-900",
              )}
            />
          </div>
        );
      })}
      {canShowButton && (
        <>
          <button
            className="absolute left-0 top-[25px] transform -translate-x-[-5px] bg-[#00000033] py-2 px-1 cursor-pointer"
            disabled={galleryIndex === 0}
            onClick={prevIndexInGallery}
          >
            <img src={arrowLeft} />
          </button>
          <button
            className="absolute right-0 top-[25px] bg-[#00000033] py-2 px-1 cursor-pointer"
            disabled={galleryIndex + limit >= images.length}
            onClick={nextIndexInGallery}
          >
            <img src={arrowRight} />
          </button>
        </>
      )}
    </div>
  );
};
