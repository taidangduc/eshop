import { useProductImage } from "../../context";
import s from "./index.module.css";
import clsx from "clsx";
import arrowLeft from "@/public/arrow_left.svg";
import arrowRight from "@/public/arrow_right.svg";
import { Image } from "@/components/ui";
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
    <div className={s["gallery-section"]}>
      {imageList.map((img, i) => {
        const __index = galleryIndex + i;
        const __active = __index === currentIndex;

        return (
          <div
            key={i}
            className={s["image-wrapper"]}
            onMouseEnter={() => selectImageByIndex(__index, img)}
            onMouseLeave={() => {}}
            onClick={() => selectImageByIndex(__index, img)}
          >
            <Image
              src={img.imageUrl}
              alt={`Thumbnail ${i}`}
              className={`${s["image-box"]} ${__active ? s["active"] : ""}`}
            />
          </div>
        );
      })}
      {canShowButton && (
        <>
          <button
            className={clsx(s["gallery__button"], s["gallery__button--left"])}
            disabled={galleryIndex === 0}
            onClick={prevIndexInGallery}
          >
            <img src={arrowLeft} />
          </button>
          <button
            className={clsx(s["gallery__button"], s["gallery__button--right"])}
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
